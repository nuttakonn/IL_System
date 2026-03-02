<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="Product_Type.aspx.cs" Inherits="ManageData_WorkProcess_Product_Type" %>
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
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
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
                        Product Type
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True"><br />
                            <table width="640" border ="0" style=" border-radius: 10px; border : 2px solid #d2e2f7; background-color:#eff4f9;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                        height: 24px; colspan="4">
                                        <asp:Label ID="lbl_AddEdit" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Add Product type" Width="150px"></asp:Label>
                                    </td>
                                </tr>
                             
                            
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                        height: 24px;">
                                        <asp:Label ID="Label2" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Product Type" Width="150px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;
                                        width: 70px">
                                        <asp:TextBox runat="server" Height="20px" Width="60px" ID="txtProductType" ></asp:TextBox>
                                        
                                    </td>
                                    <td Width="90px">
                                         <asp:Label ID="Label11" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Loan type" Width="80px" Height="16px"></asp:Label> 
                                        <asp:Label ID="Label3" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="3px" Height="16px">:</asp:Label>
                                        
                                        
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Height="20px" Width="60px" ID="txtLoanType" ></asp:TextBox>
                                    </td>
                                    <td>
                                    
                                         <asp:TextBox runat="server" Height="20px" Width="130px" ID="txtLoanTypeName" ></asp:TextBox>
                                         
                                    </td>
                                   <td Width="40px" align="center">
                                       <div id='nextBtn'>
                                           <dx:ASPxButton ID="btnProductType" runat="server" CssClass="button_bg_sky" OnClick="btnProductType_Click" BackColor="#EFF4F9" EnableDefaultAppearance="False" Cursor="pointer">
                                            <Image Url="~\Images\icon\search.png" Width="20"></Image>

                                            <Border BorderStyle="None" />
                                            <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                        </dx:ASPxButton>
                                      
                                       </div>
                                           </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 180px;
                                        vertical-align: top; height: 24px;">
                                        <asp:Label ID="label5" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; width: 180px;" Text="Product Type Description" Width="180px"
                                            Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px; vertical-align:top;">
                                        <asp:Label ID="Label8" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td colspan="5" style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                        <dx:ASPxMemo ID="txtProductTypeDescription" runat="server" Width="430px" Height="80px">
                                        </dx:ASPxMemo>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                    </td>
                                    <td colspan="5" style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                        <div>
                                            <table>
                                                <tr>
                                                    <td style="padding-left:7px;">
                                                        <dx:ASPxButton ID="btnAdd" runat="server" Text="Add" Width="70px" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                            CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                            OnClick="btnAdd_Click">
                                                            <ClientSideEvents Click="function(s, e) { 
                                                                       LoadingPanel.Show();
                                                                     }" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td style="padding-left:15px;">
                                                        <dx:ASPxButton ID="btnClearData" runat="server" Text="Clear" Width="70px" OnClick="btnClearData_Click">
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                     <asp:TextBox ID="txtSqlAll" runat="server" Visible="false"></asp:TextBox>
                                                        <asp:TextBox ID="txtProductType_D" runat="server" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                  
                                </tr>
                            </table>
                            <asp:HiddenField ID="ds_Hiddengrid" runat="server" />
                            <asp:HiddenField ID="ds_Hiddenpopup" runat="server" />
                            <br /><hr/><br />
                           <table width="100%">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px;
                                        height: 24px;">
                                        <asp:Label ID="lbl_SearchBy" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700;" Text="Search By" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px;
                                        height: 24px;">
                                        <asp:DropDownList ID="ddl_SearchBy" runat="server" Height="25px" Width="200px">
                                      <%--      <asp:ListItem Value="LC" Selected="True">Loan type</asp:ListItem>
                                            <asp:ListItem Value="LN">Loan type Name</asp:ListItem>--%>
                                            <asp:ListItem Value="PT" Selected="True">Product type</asp:ListItem>
                                            <asp:ListItem Value="PD">Product type Description</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                        height: 24px;">
                                        <asp:Label ID="lbl_SelectProductType" runat="server" Style="font-family: Tahoma;
                                            font-size: small; font-weight: 700;" Text="Select Product Type" Width="150px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label10" runat="server" Style="font-family: Tahoma; font-size: small;
                                            font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px;
                                        height: 24px;">
                                        <asp:TextBox ID="txt_ProductType" runat="server" Height="19px" Width="200px"></asp:TextBox>
                                    </td>
                                     <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btn_search" runat="server" Text="Search" Width="70px" OnClick="btn_search_Click">
                                        </dx:ASPxButton>
                                    </td>
                                     <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btn_clear" runat="server" Text="Clear" Width="70px" OnClick="btn_clear_Click">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px;
                                        height: 24px;">
                                    </td>
                                </tr>
                            </table>
                  
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="E_error" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
                                            Height="18px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                          
                          
                            <div style="width: 100%;  margin-top:-15px;">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView1_PageIndexChanging"
                                    OnSelectedIndexChanging="GridView1_SelectedIndexChanging" 
                                    OnRowDeleting="GridView1_RowDeleting">
                                   <Columns>
                                        
                                        <asp:BoundField DataField="T40LTY" HeaderText="Loan type" ReadOnly="True" SortExpression="T40LTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="T00TNM" HeaderText="Loan type Name" ReadOnly="True"
                                            SortExpression="T00TNM">
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="T40TYP" HeaderText="Product type" ReadOnly="True" SortExpression="T40TYP" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="T40DES" HeaderText="Product type description" SortExpression="T40DES" />
                                       <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                           <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                           <ItemStyle Width="60px" HorizontalAlign="Center" />
                                       </asp:ButtonField>
                                       <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                           <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                           <ItemStyle HorizontalAlign="Center" Width="60px" />
                                       </asp:CommandField>
                                    </Columns>
                                    <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                    <PagerSettings FirstPageText="First" LastPageText="Last" 
                                        Mode="NumericFirstLast" PageButtonCount="4" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <RowStyle BackColor="#F7F7DE" />
                               
                                </asp:GridView>
                            </div>
                      
                           
                            <br />
                           
                        
                            <dx:ASPxCallbackPanel ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>
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
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsg" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                      
                                        
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
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/success.png" />
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
                                Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/question.png" />

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
                                                    <dx:ASPxButton ID="btnConfirmOK" runat="server" Text="OK" Width="100px" OnClick="btnConfirmOK_Click">
                                                         
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }" />
                                                       
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnConfirmCancel_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirm.Hide(); }" />
                                                        
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
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/deletenew.png" />

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

                            <dx:ASPxPopupControl ID="Popup_AddProductType" ClientInstanceName="Popup_AddProductType"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Loan Type" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px;
                                                    height: 24px;">
                                                    <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700;" Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label12" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px;
                                                    width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddl_popup_SearchBy" runat="server" Width="150px">
                                                        <%--     <asp:ListItem Value="LC" Selected="True">Loan type</asp:ListItem>
                                                        <asp:ListItem Value="LN" >Loan type Name</asp:ListItem>--%>
                                                        <asp:ListItem Value="LT" Selected="True">Loan type</asp:ListItem>
                                                        <asp:ListItem Value="LD">Loan type Desc.</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px;
                                                    height: 24px;">
                                                    <asp:Label ID="Label13" runat="server" Style="font-family: Tahoma;
                                                        font-size: small; font-weight: 700;" Text="Select Loan Type" Width="150px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label14" runat="server" Style="font-family: Tahoma; font-size: small;
                                                        font-weight: 700; text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190; height: 24px;">
                                                    <asp:TextBox ID="txt_LoanType" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px;
                                                    height: 24px;" colspan="2">
                                                    <%--<asp:Button ID="btn_popup_search" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btn_popup_search_Click" />&nbsp;&nbsp;--%>
                                                    <dx:ASPxButton ID="btn_popup_search" runat="server" Text="Search" CssClass="AlignButtonCenter" Width="80px" Height="24px" OnClick="btn_popup_search_Click">
                                                    </dx:ASPxButton>
                                                    <%--<asp:Button ID="Button2" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF"
                                                        OnClick="btn_popup_clear_Click" />--%>
                                                    <dx:ASPxButton ID="btn_popup_clear" runat="server" Text="Clear" CssClass="AlignButtonCenter" Width="80px" Height="24px" OnClick="btn_popup_clear_Click">
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
                                       
                                        <div style="width: 100%; margin-top:-10px;">
                                            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView2_PageIndexChanging"
                                                OnSelectedIndexChanging="GridView2_SelectedIndexChanging">
                                                <Columns>
                                                 <asp:BoundField DataField="T00LTY" HeaderText="Loan type" ReadOnly="True" SortExpression="T00LTY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                                        <asp:BoundField DataField="T00TNM" HeaderText="Loan type Des." ReadOnly="True" SortExpression="T00TNM">
                                            <ItemStyle />
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
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
</asp:Content>