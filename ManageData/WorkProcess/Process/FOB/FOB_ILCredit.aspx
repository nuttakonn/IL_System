<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="FOB_ILCredit.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Process.FOB.FOB_ILCredit" %>

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
                        FOB (FAX OutBound) - ใบแจ้งผลการพิจารณาสินเชื่อผ่อนชำระ
                    </HeaderTemplate>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table style="border-style: groove; width: 100%">
                    <tr>
                        <td style="width: 138px">
                            <asp:Label ID="Label3" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Date From" Width="132px"></asp:Label>
                        </td>
                        <td style="width: 244px">
                            <asp:TextBox ID="txt_date_from" runat="server" Width="200px"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                Format="dd/MM/eeee" PopupButtonID="ImageButton1" 
                                TargetControlID="txt_date_from">
                            </cc1:CalendarExtender>
                        </td>
                        <td style="width: 110px">
                            <asp:Label ID="Label5" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Date To" 
                                Width="65px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_date_to" runat="server" Width="200px"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" 
                                Format="dd/MM/eeee" PopupButtonID="ImageButton2" TargetControlID="txt_date_to">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            <asp:Label ID="Label7" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Time From" Width="81px"></asp:Label>
                            <span style="font-size: xx-small">(HHMMSS)</span></td>
                        <td style="width: 244px">
                            <asp:TextBox ID="txt_time_from" runat="server" Width="200px" MaxLength="6"></asp:TextBox>
                        </td>
                        <td style="width: 110px">
                            <asp:Label ID="Label8" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Time To" 
                                Width="59px"></asp:Label>
                            <span style="font-size: xx-small">(HHMMSS)</span></td>
                        <td>
                            <asp:TextBox ID="txt_time_to" runat="server" Width="200px" MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            <asp:Label ID="Label10" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Business Type" Width="100px"></asp:Label>
                        </td>
                        <td style="width: 244px">
                            <asp:DropDownList ID="dd_business" runat="server" Enabled="False" Width="208px">
                                <asp:ListItem Selected="True" Value="IL">IL - Installment Loan</asp:ListItem>
                                <asp:ListItem Value="ED">Eloan - Education Loan</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            <asp:Label ID="Label26" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Send Fax Type" Width="100px"></asp:Label>
                        </td>
                        <td style="width: 244px">
                            <asp:DropDownList ID="dd_typefax" runat="server" Width="208px">
                                <asp:ListItem Selected="True" Value="PF">FAX Manual</asp:ListItem>
                                <asp:ListItem Value="RP">Reprint FAX Manual</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            &nbsp;</td>
                        <td style="width: 244px">
                            &nbsp;</td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>  
                        <td>
                            <asp:CheckBox ID="chkSpecial" AutoPostBack="True" runat="server" Font-Bold="True" ForeColor="Red" 
                                Text=" Special Print FAX" OnCheckedChanged="chkSpecial_CheckedChanged" />
                        
                        </td>
                        <td>
                            &nbsp;</td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Height="16px" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Vendor Code" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_vendor" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" runat="server" Height="16px" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Application No." Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_application" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label24" runat="server" Height="16px" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Branch No." Width="100px"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="dd_brn" runat="server" ValueType="System.String" 
                                Width="208px">
                                <DisabledStyle BackColor="Silver">
                                </DisabledStyle>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="width: 110px">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            &nbsp;</td>
                        <td style="width: 244px">
                            <dx:ASPxButton ID="btn_search" runat="server" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003Blue" Height="24px" 
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Print FAX" 
                                Width="113px" OnClick="btn_search_Click">
                                <ClientSideEvents Click="function(s, e) {LoadingPanel.Show();}" />
                            </dx:ASPxButton>
                        </td>
                        <td style="width: 110px">
                            <dx:ASPxButton ID="btn_clear" runat="server" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003Blue" Height="24px" 
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Reset" 
                                Width="113px" OnClick="btn_clear_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            &nbsp;</td>
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
                <dx:ASPxPopupControl ID="PopupConfirmSave" ClientInstanceName="PopupConfirmSave" ShowPageScrollbarWhenModal="true"
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
                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px">
                    </Paddings>
                    </HeaderStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" 
                    SupportsDisabledAttribute="True">
                            <table width="100%" align="center" >
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
                            <asp:HiddenField ID="hid_oper" runat="server" />
                            <br />
                            <br />
                            <br />
                            <table width="100%" align="center">
                                <tr>
                                    <td align="right" width="50%" >
                                        <dx:ASPxButton ID="btnConfirmSave" runat="server" Text="OK" Width="100px" 
                                    OnClick="btnConfirmSave_Click" >
                                            <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
                                            <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }">
                                            </ClientSideEvents>
                                        </dx:ASPxButton>
                                    </td>
                                    <td align="left" width="50%" >
                                        <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" >
                                            <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
                                            <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();}">
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
