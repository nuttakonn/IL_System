<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="ProductModel.aspx.cs" Inherits="ManageData_WorkProcess_ProductModel" %>
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
    <script type="text/javascript">
        function ShowAdvanceSearchPopup() {
            var advance = document.getElementById("hdfAdvanceSearch");
            if (advance.value == "Y") {
                PopupAdvanceSearch.Show();
            }
            else {
                PopupAdvanceSearch.Hide();
            }
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
                        Product Model
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <asp:HiddenField ID="ds_Hiddengrid" runat="server" />
                            <asp:HiddenField ID="ds_popup_type" runat="server" />
                            <asp:HiddenField ID="ds_popup_code" runat="server" />
                            <asp:HiddenField ID="ds_popup_brand" runat="server" />
                            <table width="100%" cellpadding="5" style="border-radius: 10px; border:2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <asp:Label ID="lbl_SearchBy" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Search By" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                        <asp:DropDownList ID="ddl_SearchBy" runat="server" Width="200px">
                                            <asp:ListItem Value="PTC" Selected="True">Product Type Code</asp:ListItem>
                                            <asp:ListItem Value="PTD">Product Type Description</asp:ListItem>
                                            <asp:ListItem Value="PC">Product Code</asp:ListItem>
                                            <asp:ListItem Value="PD">Product Description</asp:ListItem>
                                            <asp:ListItem Value="BC">Brand Code</asp:ListItem>
                                            <asp:ListItem Value="BN">Brand Name</asp:ListItem>
                                            <asp:ListItem Value="MC">Model Code</asp:ListItem>
                                            <asp:ListItem Value="MD">Model Description</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                        <asp:Label ID="Label8" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Select Product Model" Width="150px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 280px; height: 24px;">
                                        <asp:TextBox ID="txt_ProductModel" runat="server" Width="250px"></asp:TextBox>&nbsp;&nbsp;
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
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px; height: 24px;"></td>
                                </tr>
                                <tr>
                                    <td colspan="10">
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
                                                <div style="width: 100%; height:auto; min-height:420px; text-align: right; ">
                                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                        AllowPaging="True" EmptyDataText="No records found" ShowHeaderWhenEmpty="true"
                                                        OnPageIndexChanging="GridView1_PageIndexChanging"
                                                        OnSelectedIndexChanging="GridView1_SelectedIndexChanging"
                                                        OnRowDeleting="GridView1_RowDeleting">
                                                        <Columns>
                                                            <asp:BoundField DataField="T43LTY" HeaderText="Loan Type Code" ReadOnly="True" SortExpression="T43LTY"
                                                                Visible="false">
                                                                <ItemStyle Width="80px" HorizontalAlign="right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T00TNM" HeaderText="Loan Type Name" ReadOnly="True" Visible="false"
                                                                SortExpression="T00TNM">
                                                                <ItemStyle Width="120px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T43TYP" HeaderText="Product Type Code" ReadOnly="True" SortExpression="T43TYP">
                                                                <ItemStyle Width="80px" HorizontalAlign="right" />
                                                                <ItemStyle Width="95px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" ReadOnly="True"
                                                                SortExpression="T40DES">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>                                                
                                                            <asp:BoundField DataField="T43COD" HeaderText="Product Code" ReadOnly="True" SortExpression="T43COD">
                                                                <ItemStyle Width="100px" HorizontalAlign="right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T41DES" HeaderText="Product Description" ReadOnly="True" SortExpression="T41DES">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T43BRD" HeaderText="Brand Code" ReadOnly="True" SortExpression="T43BRD">
                                                                <ItemStyle Width="80px" HorizontalAlign="right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T42DES" HeaderText="Brand Name" ReadOnly="True" SortExpression="T42DES">
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                               
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T43MDL" HeaderText="Model Code" ReadOnly="True" SortExpression="T43MDL">
                                                                <ItemStyle Width="80px" HorizontalAlign="right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="T43DES" HeaderText="Model Description" SortExpression="T43DES" />
                                                            <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                            </asp:ButtonField>

                                                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
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
                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9;">
                                <tr>
                                    <td align="left" width="350px" height="24px" style="border-radius: 8px 8px 0px 0px; border:2px solid #D2E2F7; background-color: #D2E2F7;" colspan="6">
                                        <asp:Label ID="lblAddEdit" runat="server" Text="Add"></asp:Label>&nbsp;Product Model
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                        height: 24px;">
                                        <asp:Label ID="Label3" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Loan Type" Width="120px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                        <asp:Label ID="lblLoanType" runat="server" Width="70px" Font-Bold="true" ForeColor="Blue"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblLoanTypeDesc" runat="server" Width="150px" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                        height: 24px;">
                                        <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Product Type" Width="120px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;
                                        height: 24px;">
                                        <asp:TextBox ID="txtProductType" runat="server" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtProductTypeDesc" runat="server" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                        <asp:ImageButton Height="18" id="btnSelectProductType" runat="server" AlternateText="ImageButton" ToolTip="Find Product Type"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnSelectProductTypeClick"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                        height: 24px;">
                                        <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Product Code" Width="120px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;
                                        height: 24px;">
                                        <asp:TextBox ID="txtProductCode" runat="server" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtProductDesc" runat="server" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                        <asp:ImageButton Height="18" id="btnSelectProductCode" runat="server" AlternateText="ImageButton" ToolTip="Find Product Code"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnSelectProductCodeClick"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                        height: 24px;">
                                        <asp:Label ID="Label2" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Brand" Width="120px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 500px;
                                        height: 24px;">
                                        <asp:TextBox ID="txtBrandCode" runat="server" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:TextBox ID="txtBrandName" runat="server" Width="300px" Enabled="false"></asp:TextBox>&nbsp;
                                        <asp:ImageButton Height="18" id="btnSelectBrand" runat="server" AlternateText="ImageButton" ToolTip="Find Brand"
                                            ImageUrl="~\Images\icon\search.png" OnClick="btnSelectBrandClick"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px;
                                        height: 24px;">
                                        <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Model" Width="120px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px;
                                        height: 24px;">
                                        <asp:TextBox ID="txtProductModelCode" runat="server" Width="70px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" style="text-align: left; font-family: Tahoma; font-size: small; width: 400px; height: 24px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; height: 24px; vertical-align: top;">
                                        <asp:Label ID="Label16" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Model Desc." Width="120px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 300px; height: 24px;">
                                        <asp:TextBox ID="txtProductModelDesc" runat="server" Rows="2" TextMode="MultiLine" Width="380px" Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btnAddProductModel" runat="server" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                        CssPostfix="Office2003Blue" OnClick="btnAddProductModelClick" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                        Text="Add" Width="70px">
                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btnClearData" runat="server" OnClick="btnClearDataClick" Text="Clear"  Width="70px">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSqlAll" runat="server" Visible="False" Width="250px"></asp:TextBox>
                                                    <asp:TextBox ID="txtProductModel_D" runat="server" Visible="False" Width="250px"></asp:TextBox>
                                                    <asp:TextBox ID="pProType" runat="server" Visible="False" Width="250px"></asp:TextBox>
                                                    <asp:TextBox ID="pProBrand" runat="server" Visible="False" Width="250px"></asp:TextBox>
                                                    <asp:TextBox ID="pProCode" runat="server" Visible="False" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Height="150px" Width="250px"></dx1:ASPxLoadingPanel>

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
                                                    <dx:ASPxButton ID="btnPopupMessageOK" runat="server" Text="OK" Width="100px" OnClick="btnPopupMessageOKClick">
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
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server" SupportsDisabledAttribute="True">
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

                            <dx:ASPxPopupControl ID="PopupConfirm" ClientInstanceName="PopupConfirm" ShowPageScrollbarWhenModal="true"
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
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmAddOKEdit" runat="server" Text="OK" Width="100px" OnClick="btnConfirmAddEditOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmAddEditCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnConfirmAddEditCancelClick">
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
                                PopupVerticalAlign="WindowCenter" HeaderText="Confirm Delete" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css"
                                RenderMode="Lightweight">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
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
                                                    <dx:ASPxLabel ID="lblConfimMsg_Delete" runat="server" Font-Names="Tahoma" Font-Size="Small"
                                                        Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnDeleteConfirmOK" runat="server" Text="OK" Width="100px" OnClick="btnDeleteConfirmOKClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnDeleteConfirmCancel" runat="server" Text="Cancel" Width="100px"
                                                        OnClick="btnDeleteConfirmCancelClick">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <dx:ASPxPopupControl ID="PopupAddProductType" ClientInstanceName="PopupAddProductType"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Type" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { ShowAdvanceSearchPopup(); }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px;
                                                    height: 24px;">
                                                    <asp:Label ID="Label17" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label18" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px;
                                                    width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductTypeSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PT" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PD">Product Type Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="lbl_SelectProductType" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Select Product Type" Width="140px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label19" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductTypeSearchText" runat="server" Width="185px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupProductTypeSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupProductTypeSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupProductTypeClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupProductTypeClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView2_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView2_SelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T40LTY" HeaderText="Loan Type Code" ReadOnly="True" SortExpression="ILTB40" Visible="false">
                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LOANTYPENAME" HeaderText="Loan Type Name" ReadOnly="True" Visible="false" SortExpression="ILTB40">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40TYP" HeaderText="Product Type Code" ReadOnly="True" SortExpression="ILTB40">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" SortExpression="ILTB40" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" buttontype="Image" ImageUrl="~\Images\icon\click_select.png"   ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
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

                            <dx:ASPxPopupControl ID="PopupAddBrand" ClientInstanceName="PopupAddBrand" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Brand" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="850px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"> </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { ShowAdvanceSearchPopup(); }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px;
                                                    height: 24px;">
                                                    <asp:Label ID="Label20" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label21" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px;
                                                    width: 160px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddBrandSearchBy" runat="server" Width="140px">
                                                        <asp:ListItem Value="BrandCode" Selected="True">Brand Code</asp:ListItem>
                                                        <asp:ListItem Value="BrandName">Brand Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 130px;
                                                    height: 24px;">
                                                    <asp:Label ID="Label22" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Select Brand" Width="120px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label23" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddBrandSearchText" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btnPopupBrandSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupBrandSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupBrandClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupBrandClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;  text-align: right;">
                                            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView3_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView3_SelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T42BRD" HeaderText="Brand Code" ReadOnly="True" SortExpression="T42BRD">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T42DES" HeaderText="Brand Name" SortExpression="T42DES" />
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
                                            <asp:Label ID ="lblGridView3" runat="server"></asp:Label>
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <dx:ASPxPopupControl ID="PopupAddProductCode" ClientInstanceName="PopupAddProductCode"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Code" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <ClientSideEvents CloseUp="function(s, e) { ShowAdvanceSearchPopup(); }" /> 
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                            <tr>
                                                <td align="left" width="70px" height="24px">
                                                    <asp:Label ID="Label25" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label26" runat="server">:</asp:Label>
                                                </td>
                                                <td align="left" width="180px" height="24px">
                                                    <asp:DropDownList ID="ddlPopupAddProductCodeSearchBy" runat="server" Width="160px">
                                                        <asp:ListItem Value="PT" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PC">Product Code</asp:ListItem>
                                                        <asp:ListItem Value="PD">Product Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="150px" height="24px">
                                                    <asp:Label ID="Label27" runat="server" Text="Select Product Type"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label28" runat="server">:</asp:Label>
                                                </td>
                                                <td align="left" width="150px" height="24px">
                                                    <asp:TextBox ID="txtPopupAddProductCodeSearchText" runat="server" Width="180px"></asp:TextBox>
                                                </td>
                                                <td align="left" height="24px">
                                                    <dx:ASPxButton ID="btnPopupProductCodeSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupProductCodeSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupProductCodeClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupProductCodeClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView4_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView4_SelectedIndexChanging">
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
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" ReadOnly="True" Visible="false" SortExpression="T40DES">
                                                        <ItemStyle Width="200px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41COD" HeaderText="Product Code" ReadOnly="True" SortExpression="T41COD">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41DES" HeaderText="Product Description" SortExpression="T41DES" />
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

                            <dx:ASPxPopupControl ID="PopupAdvanceSearch" ClientInstanceName="PopupAdvanceSearch"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Advance Search" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="700px" Height="250px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                            <tr>
                                                <td align="left" width="120px" height="24px">
                                                    <asp:Label ID="Label34" runat="server" Text="Product Type"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label35" runat="server">:</asp:Label>
                                                </td>
                                                <td align="left" width="500px" height="24px">
                                                    <asp:TextBox ID="txtAdvanceSearchProductTypeCode" runat="server" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtAdvanceSearchProductTypeDesc" runat="server" Width="280px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="18" id="btnAdvanceSearchProductType" runat="server" AlternateText="ImageButton" ToolTip="Find Product Type"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnAdvanceSearchProductTypeClick"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="24px">
                                                    <asp:Label ID="Label36" runat="server" Text="Product Code"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label37" runat="server">:</asp:Label>
                                                </td>
                                                <td align="left" width="500px" height="24px">
                                                    <asp:TextBox ID="txtAdvanceSearchProductCode" runat="server" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtAdvanceSearchProductDesc" runat="server" Width="280px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="18" id="btnAdvanceSearchProductCode" runat="server" AlternateText="ImageButton" ToolTip="Find Product Code"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnAdvanceSearchProductCodeClick"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="24px"">
                                                    <asp:Label ID="Label38" runat="server" Text="Brand"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label39" runat="server">:</asp:Label>
                                                </td>
                                                <td align="left" width="500px" height="24px">
                                                    <asp:TextBox ID="txtAdvanceSearchBrandCode" runat="server" Width="70px" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtAdvanceSearchBrandName" runat="server" Width="280px" Enabled="false"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton Height="18" ID="btnAdvanceSearchBrand" runat="server" AlternateText="ImageButton" ToolTip="Find Brand"
                                                        ImageUrl="~\Images\icon\search.png" OnClick="btnAdvanceSearchBrandClick" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="120px" height="24px">
                                                    <asp:Label ID="Label40" runat="server" Text="Model"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label41" runat="server">:</asp:Label>
                                                </td>
                                                <td align="left" width="500px" height="24px">
                                                    <asp:TextBox ID="txtAdvanceSearchModelCode" runat="server" Width="70px"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtAdvanceSearchModelDesc" runat="server" Width="280px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"></td>
                                                <td colspan="2" align="left" height="24px" width="500px">
                                                    <dx:ASPxButton ID="btnPopupAdvanceSearch" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAdvanceSearchClick"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="btnPopupAdvanceSearchClear" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"
                                                        OnClick="btnPopupAdvanceSearchClearClick"></dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            
                            <asp:HiddenField ID="hdfAdvanceSearch" ClientIDMode="Static" runat="server" />
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
</asp:Content>
