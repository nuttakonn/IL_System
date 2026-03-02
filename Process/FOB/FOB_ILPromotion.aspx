<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="FOB_ILPromotion.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Process.FOB.FOB_ILPromotion" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phContents" Runat="Server">

    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    GroupBoxCaptionOffsetY="-28px" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100%" 
                    Height="136px">
        <ContentPaddings Padding="5px" />
        <ContentPaddings Padding="5px">
         
        </ContentPaddings>
        
        <HeaderTemplate>
                        FOB IL Promotion
                    </HeaderTemplate>
        <PanelCollection>
            
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table style="border-style: groove; width: 100%">
                    <tr>
                    <td colspan="6">
                        <asp:Panel ID="Panel1"  runat="server" 
                            GroupingText="ประเภทจดหมาย (Letter Type)" BackColor="White">
                            <asp:HiddenField runat="server" ID="ds_gridProduct" />

                            <table style="width: 100%">
                                <tr>
                                    <td style="width:100%">
                                        <table style="width:100%">
                                        <tr>
                                            <td style="width: 397px" rowspan="4">
                                                <asp:RadioButtonList ID="rd_LetterType" runat="server" AutoPostBack="True" 
                                                    OnSelectedIndexChanged="rd_LetterType_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">จดหมายการจัดรายการส่งเสริมการขาย (ระบุรุ่น)</asp:ListItem>
                                                    <asp:ListItem Value="1">จดหมายการจัดรายการส่งเสริมการขาย อัตราดอกเบี้ย 2 ช่วง (ไม่ระบุรุ่น)</asp:ListItem>
                                                    <asp:ListItem Value="2">จดหมายการจัดรายการส่งเสริมการขาย อัตราดอกเบี้ย 1 ช่วง (ไม่ระบุรุ่น)</asp:ListItem>
                                                    <asp:ListItem Value="3">จดหมายการจัดรายการส่งเสริมการขาย อัตราดอกเบี้ยปกติ (Type3)</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            <asp:Label ID="Label5" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Letter Type :" Width="160px"></asp:Label>
                        </td>
                        <td colspan=2 style="width: 104px">
                            <dx:ASPxComboBox ID="dd_lettertype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dd_lettertype_SelectedIndexChanged" 
                                ValueType="System.String" Width="208px">
                                <DisabledStyle BackColor="Silver">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 313px">
                            &nbsp;</td>
                        <td style="width: 369px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            <asp:Label ID="Label3" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Promotion Start Date :" Width="161px"></asp:Label>

                        </td>
                        <td colspan=2 style="width: 104px">
                            <asp:TextBox ID="txt_date" runat="server" Width="200px"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                Format="dd/MM/eeee" PopupButtonID="ImageButton1" TargetControlID="txt_date">
                            </cc1:CalendarExtender>

                        </td>
                        <td style="width: 313px">
                            &nbsp;</td>
                        <td style="width: 369px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            <asp:Label ID="Label4" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Voicher Detail :" Width="161px"></asp:Label>
                        </td>
                        <td colspan="2" style="width: 104px">
                            <asp:TextBox ID="txtVoicher" runat="server" Rows="4" TextMode="MultiLine" 
                                Width="391px"></asp:TextBox>
                        </td>
                        <td style="width: 313px">
                            &nbsp;</td>
                        <td style="width: 369px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            <asp:Label ID="Label2" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Campaign :" Width="160px"></asp:Label>
                        </td>
                        <td colspan=2 style="width: 104px">
                            <dx:ASPxComboBox ID="dd_campaign" runat="server" AutoPostBack="True" 
                                OnSelectedIndexChanged="dd_campaign_SelectedIndexChanged" 
                                ValueType="System.String" Width="208px" Enabled="False">
                                <DisabledStyle BackColor="Silver">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 313px">
                            &nbsp;</td>
                        <td style="width: 369px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            <asp:Label ID="Label1" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Vendor :" Width="160px"></asp:Label>
                        </td>
                        <td colspan=2 style="width: 104px">
                            <dx:ASPxComboBox ID="dd_vendor" runat="server" AutoPostBack="True" 
                                ValueType="System.String" Width="208px" Enabled="False" 
                                OnSelectedIndexChanged="dd_vendor_SelectedIndexChanged">
                                <DisabledStyle BackColor="Silver">
                                </DisabledStyle>
                                <ClientSideEvents ValueChanged="function(s, e) {LoadingPanel.Show();}" />
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 313px">
                            &nbsp;</td>
                        <td style="width: 369px">
                            &nbsp;</td>
                    </tr>
                    <%--<tr>
                        <td style="background-color: #66CCFF;">
                            <asp:Label ID="Label8" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Vendor :" Width="161px"></asp:Label>

                        </td>
                        <td colspan=2 style="background-color: #66CCFF;">
                            &nbsp;</td>
                        <td style="background-color: #66CCFF;">
                            &nbsp;</td>
                        <td style="background-color: #66CCFF;">
                            &nbsp;</td>
                    </tr>
                       <tr>
                           <td style="width: 142px">
                               &nbsp;</td>
                           <td colspan="2" style="width: 104px">
                               <asp:GridView ID="grdVendor" runat="server" AllowPaging="True" 
                                   BorderStyle="Groove" CellPadding="4" ForeColor="#333333" 
                                   OnPageIndexChanging="grdVendor_PageIndexChanging" 
                                   OnRowDataBound="grdVendor_RowDataBound">
                                   <AlternatingRowStyle BackColor="White" />
                                   <EditRowStyle BackColor="#2461BF" />
                                   <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                   <PagerSettings FirstPageText="First" LastPageText="Last" />
                                   <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" />
                                   <RowStyle BackColor="#EFF3FB" />
                                   <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                   <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                   <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                   <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                   <SortedDescendingHeaderStyle BackColor="#4870BE" />
                               </asp:GridView>
                           </td>
                           <td style="width: 313px">
                               &nbsp;</td>
                           <td style="width: 369px">
                               &nbsp;</td>
                    </tr>--%>
                       <tr>
                        <td  colspan="6"  style="background-color: #66CCFF;">
                            <asp:Label ID="Label9" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Product :" Width="161px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                    <td style="width: 142px">
                            &nbsp;&nbsp;</td>
                        <td colspan="5" style="width: auto">
                            <asp:GridView ID="grdProduct" runat="server" AllowPaging="True" 
                                BorderStyle="Groove" CellPadding="4" ForeColor="#333333" 
                                OnPageIndexChanging="grdProduct_PageIndexChanging" 
                                OnRowDataBound="grdProduct_RowDataBound"
                                AutoGenerateColumns="False" >
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                <Columns>
                                  <asp:TemplateField ItemStyle-Width="5%" HeaderText="No." 
                                  ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-Width="15%" HeaderText="ยี่ห้อ/BRAND" 
                                  ItemStyle-HorizontalAlign="Left">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("BRAND")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-Width="15%" HeaderText="สินค้า/PRODUCT" 
                                  ItemStyle-HorizontalAlign="Left">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("PRODUCT")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="20%" HeaderText="รุ่น/MODEL" 
                                  ItemStyle-HorizontalAlign="Left">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("MODEL")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="PRO.ITEM" 
                                  ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("PROITEM", "{0:000000}")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="ราคา(รวมภาษี)" 
                                  ItemStyle-HorizontalAlign="Right">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("PRICE", "{0:#,##0.00}")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="ดอกเบี้ย(%)" 
                                  ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("INT")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="ค่าธรรมเนียม(%)" 
                                  ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("CRU")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="จากระยะเวลา(เดือน)" 
                                  ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("PERIODFROM")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField ItemStyle-Width="10%" HeaderText="ถึงระยะเวลา(เดือน)" 
                                  ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                         <asp:Label runat="server" Text='<%#Eval("PERIODTO")%>'></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                             </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            &nbsp;&nbsp;</td>
                        <td style="width: 104px">
                            <dx:ASPxButton ID="btn_preview" runat="server" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003Blue" Height="24px" OnClick="btn_preview_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Preview" 
                                Width="113px">
                                <ClientSideEvents Click="function(s, e) {LoadingPanel.Show();}" />
                            </dx:ASPxButton>
                        </td>
                        <td style="width: 104px">
                            <dx:ASPxButton ID="btn_reset" runat="server" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003Blue" Height="24px" OnClick="btn_reset_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Reset" 
                                Width="113px">
                            </dx:ASPxButton>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"  Text="TEST"
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" Width="608px"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 142px">
                            &nbsp;</td>
                        <td style="width: 104px">
                            &nbsp;&nbsp;</td>
                        <td style="width: 104px">
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br/>
                <br />
                <dx:ASPxPopupControl ID="PopupMsg" ClientInstanceName="PopupMsg" ShowPageScrollbarWhenModal="true"
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
                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                    </Paddings>
                    </HeaderStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                            <table width="100%" align="center" >
                                <tr>
                                    <td align="center">
                                        <dx:ASPxLabel ID="lblMsgTH" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #CC0000">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <dx:ASPxLabel ID="lblMsgEN" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <br />
                            <br />
                            <table width="100%" align="center">
                                <tr>
                                    <td align="center" >
                                        <dx:ASPxButton ID="btnClosePopupMsg" runat="server" Text="OK" Width="100px" >
                                            <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }" />
                                            <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }">
                                            </ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx1:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
          ClientInstanceName="LoadingPanel" Height="150px" Width="250px">
                </dx1:ASPxLoadingPanel>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>

</asp:Content>
