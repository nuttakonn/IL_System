<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="CampaignProposal.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Campaign.CampaignProposal" %>
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
    <script type="text/javascript" src="../../../Js/ValidateNumber.js"></script>
    <script type="text/javascript">  
        function OnClientClickEventHandler(s, e) {
            LoadingPanel.Show();
        }
        function OnCallbackComplete(s, e) {
            LoadingPanel.Hide();
        }
    </script>
    <script type="text/javascript">
        function TRANFER_REPORT_PRINT() {
            window.open("/ManageData/WorkProcess/Campaign/REPORT/reportCampaignProposal.aspx");
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
                        Campaign Proposal
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table width="100%" border="0">
                                <asp:HiddenField ID="ds_gvCampaign" runat="server" />
                                <tr style="height: 30px;">
                                    <td style="width: 120px;">
                                        <asp:Label ID="Label17" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Select Campaign" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:TextBox runat="server" Height="20px" Width="100px" ID="campaignCode" OnTextChanged="SELECT_CAMPAIGN_ONCHANGE" autocomplete="off" AutoPostBack="true" MaxLength="13" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                        <asp:HiddenField ID="ds_gvListData" runat="server" />
                                    </td>
                                    <td style="width: 30px;" align="center">
                                        <asp:ImageButton Height="18" ID="ImageButton2" runat="server" AlternateText="ImageButton" ImageAlign="left" ImageUrl="~\Images\icon\search.png" OnClick="CLICK_SEARCH_CAMPAIGN" />
                                    </td>
                                    <td style="width: 180px;" colspan="4">
                                        <asp:TextBox runat="server" Height="20px" Width="180px" ID="campaignDes"></asp:TextBox>

                                    </td>

                                    <td style="width: 30px;"></td>

                                    <td colspan="3">
                                        <dx:ASPxButton ID="btnReport" runat="server" value="modeSave" Text="Report Preview" CssClass="imgNormal2" Cursor="pointer" OnClick="REPORT_PRINT">
                                            <Image Url="~\Images\icon\print.png" Width="16"></Image>
                                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                            <DisabledStyle CssClass="imgGreyscal">
                                            </DisabledStyle>
                                        </dx:ASPxButton>

                                    </td>

                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr style="height: 30px;">
                                    <td style="width: 140px;">
                                        <asp:Label ID="Label18" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Type" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="width: 140px;" colspan="2">
                                        <asp:TextBox ID="txtType" runat="server" Width="130px" Height="20"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:Label ID="Label1" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Rate" Width="40px"></asp:Label>
                                        :
                                        <asp:TextBox runat="server" Height="20px" Width="40px" ID="txtRate" Style="text-align: center"></asp:TextBox>
                                        %
                                    </td>
                                    <td style="width: 80px; padding-left: 30px;" colspan="4">

                                        <asp:Label ID="Label4" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Start Date" Width="80px"></asp:Label>
                                    </td>
                                    <td width="10">:
            
                                    </td>
                                    <td width="150">
                                        <asp:TextBox runat="server" Height="20px" Width="109px" ID="startDate" Style="text-align: center"></asp:TextBox>

                                    </td>
                                    <td style="width: 120px;">

                                        <asp:Label ID="Label3" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Closing App. Date" Width="120px"></asp:Label>
                                    </td>
                                    <td width="10">:</td>
                                    <td>
                                        <asp:TextBox runat="server" Height="20px" Width="109px" ID="closeAppDate" Style="text-align: center"></asp:TextBox></td>
                                    <td>&nbsp;</td>

                                </tr>
                                <tr style="height: 30px;">
                                    <td style="width: 120px;">
                                        <asp:Label ID="Label19" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Subsidize by" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="width: 250px;" colspan="3">
                                        <asp:TextBox runat="server" Height="20px" Width="239px" ID="txtSubsidize"></asp:TextBox>
                                    </td>




                                    <td style="width: 80px; padding-left: 30px;" colspan="4">

                                        <asp:Label ID="Label2" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="End Date" Width="80px"></asp:Label>
                                    </td>
                                    <td width="10">:</td>
                                    <td width="150">
                                        <asp:TextBox runat="server" Height="20px" Width="109px" ID="endDate" Style="text-align: center"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">

                                        <asp:Label ID="Label5" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Closing Lay Bill" Width="120px"></asp:Label>
                                    </td>
                                    <td width="10">:</td>
                                    <td>
                                        <asp:TextBox runat="server" Height="20px" Width="109px" ID="closeLayDate" Style="text-align: center"></asp:TextBox></td>
                                    <td>&nbsp;</td>



                                </tr>
                            </table>
                            <br />
                            <hr />
                            <table width="100%" border="0" style="border: 2px double #FFFFFF; display: inline-block; white-space: nowrap;">
                                <tr>
                                    <td colspan="2">
                                        <table width="900">

                                            <tr>

                                                <td align="left">
                                                    <table width="804" class="bg4">
                                                        <tr style="height: 20px;">
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="Label11" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                    Text="Note List" Width="130px" Height="16px"></asp:Label></td>
                                                        </tr>

                                                    </table>
                                                    <div style="height: 150px; width: 804px; overflow-y: scroll; background-color: #F8F8F8;">

                                                        <asp:GridView ID="gvListData" runat="server" CssClass="Grid" EmptyDataText="No records found" AutoGenerateColumns="False" Width="100%"
                                                            EnableModelValidation="True" ForeColor="#333333" ShowHeaderWhenEmpty="True">
                                                            <AlternatingRowStyle BackColor="White" />

                                                            <Columns>

                                                                <asp:BoundField DataField="C11UDT" HeaderText="NOTE DATE" ItemStyle-Width="100">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="134"></HeaderStyle>
                                                                    <ItemStyle Width="134px" HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C11UTM" HeaderText="NOTE TIME">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="134"></HeaderStyle>
                                                                    <ItemStyle Width="134px" HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C11NSQ" HeaderText="SEQ" ItemStyle-Width="100">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="83"></HeaderStyle>
                                                                    <ItemStyle Width="83px" HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C11NOT" HeaderText="NOTE DESCRIPTION">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="450"></HeaderStyle>
                                                                    <ItemStyle Width="450px" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>

                                                            </Columns>
                                                            <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                            <FooterStyle BackColor="#990000" ForeColor="White" />
                                                            <HeaderStyle BackColor="#83B2D1" ForeColor="#000000" />
                                                            <PagerStyle BackColor="#F8F8F8" ForeColor="#333333" HorizontalAlign="Center" />
                                                            <%--<RowStyle BackColor="#FFFFD0" ForeColor="#333333" />--%>
                                                            <SelectedRowStyle BackColor="#CAFFE3" />


                                                            <SortedDescendingCellStyle BackColor="#FCF6C0" />

                                                        </asp:GridView>
                                                    </div>

                                                </td>
                                            </tr>
                                        </table>


                                    </td>
                                </tr>
                                <tr>
                                    <td width="35%" align="left">
                                        <table width="600" class="bg4">
                                            <tr style="height: 20px;">
                                                <td style="padding-left: 5px;">
                                                    <asp:Label ID="Label29" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Vendor List" Width="130px" Height="16px"></asp:Label></td>
                                            </tr>

                                        </table>

                                        <div style="height: 300px; width: 600px; overflow-y: scroll; overflow-x: scroll; background-color: #E8FFFF;">

                                            <asp:GridView ID="gvListDataVendor" OnRowDataBound="GV_LIST_VENDOR_ROWDATABOUND" runat="server" CssClass="Grid" EmptyDataText="No records found" AutoGenerateColumns="False" Width="700"
                                                EnableModelValidation="True" ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle BackColor="White" />

                                                <Columns>

                                                    <asp:BoundField DataField="C08VEN1" HeaderText="VEND ID" ItemStyle-Width="100">
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="P10NAM" HeaderText="VEND NAME">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="P16RNK" HeaderText="RANK" ItemStyle-Width="50">
                                                        <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
                                                        <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="P12WOR" HeaderText="WO(%)">
                                                        <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                                                        <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="P12ODR" HeaderText="OD(%)">
                                                        <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                                                        <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="P10EDT" HeaderText="EXPIRED">
                                                        <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                                                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C81END" HeaderText="ADJ END">
                                                        <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                                                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <FooterStyle BackColor="#990000" ForeColor="White" />
                                                <HeaderStyle BackColor="#83B2D1" ForeColor="#000000" />
                                                <PagerStyle BackColor="#CAFFE3" ForeColor="#333333" HorizontalAlign="Center" />
                                                <%--<RowStyle BackColor="#C9FECC" ForeColor="#333333" />--%>
                                                <SelectedRowStyle BackColor="#CAFFE3" />


                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />

                                            </asp:GridView>
                                        </div>


                                    </td>
                                    <td width="65%">
                                        <table width="700" class="bg4">
                                            <tr style="height: 20px;">
                                                <td style="padding-left: 5px;">
                                                    <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Campaign List" Width="130px" Height="16px"></asp:Label></td>
                                            </tr>

                                        </table>


                                        <div style="height: 300px; width: 700px; overflow-y: scroll; background-color: #FFECFF">

                                            <asp:GridView ID="gvListDataCampaign" runat="server" CssClass="Grid" EmptyDataText="No records found" OnRowDataBound="GV_LIST_CAMPAIGN_ROWDATABOUND" AutoGenerateColumns="False" Width="100%"
                                                EnableModelValidation="True" ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle BackColor="White" />

                                                <Columns>

                                                    <asp:BoundField DataField="TA" HeaderText="BRAND">
                                                        <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>
                                                        <ItemStyle Width="80px" HorizontalAlign="left"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TB" HeaderText="PRODUCT CODE">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TC" HeaderText="MODEL">
                                                        <HeaderStyle HorizontalAlign="Center" Width="200px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="left" Width="200px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C07A" HeaderText="MIN PRICE">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C07B" HeaderText="MAX PRICE">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C02A" HeaderText="INT RATE%">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C02B" HeaderText="CRR USG%">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C02C" HeaderText="INF RATE%">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C02D" HeaderText="TERM BEGIN">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C02E" HeaderText="TERM END">
                                                        <HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
                                                        <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="C02F" HeaderText="INSTALL">
                                                        <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
                                                        <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <FooterStyle BackColor="#990000" ForeColor="White" />
                                                <HeaderStyle BackColor="#83B2D1" ForeColor="#000000" />
                                                <PagerStyle BackColor="#CAFFE3" ForeColor="#333333" HorizontalAlign="Center" />
                                                <%--<RowStyle BackColor="#FEDCDA" ForeColor="#333333" />--%>
                                                <SelectedRowStyle BackColor="#CAFFE3" />


                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />

                                            </asp:GridView>
                                        </div>

                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
    <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server"
        ClientInstanceName="LoadingPanel" Height="170px" Width="320px">
    </dx1:ASPxLoadingPanel>

    <dx:ASPxPopupControl ID="POPUP_ALERT_CHECK" ClientInstanceName="POPUP_ALERT_CHECK"
        ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="Middle" HeaderText=""
        AllowDragging="True" Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
        CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" CloseAction="CloseButton">
        <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
        </LoadingPanelImage>
        <ContentStyle HorizontalAlign="center" VerticalAlign="Top">
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
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/alert.png" />

                        </td>
                    </tr>
                    <tr>
                        <td align="center">

                            <dx:ASPxLabel ID="lblMsgAlert" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                </table>


                <br />
                <table width="100%" align="center">
                    <tr>
                        <td align="center" width="50%">
                            <dx:ASPxButton ID="popupAlerCheck" runat="server" Text="OK" Width="100px">

                                <ClientSideEvents Click="function(s, e) { POPUP_ALERT_CHECK.Hide(); }"></ClientSideEvents>
                            </dx:ASPxButton>
                        </td>

                    </tr>

                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="POPUP_MSG_ERROR" ClientInstanceName="POPUP_MSG_ERROR" ShowPageScrollbarWhenModal="true"
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
            <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server" SupportsDisabledAttribute="True">
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

                <table width="100%" align="center">
                    <tr>
                        <td align="center" width="50%">
                            <br />
                            <dx:ASPxButton ID="btnOK" runat="server" Text="OK" Width="100px">
                                <ClientSideEvents Click="function(s, e) { POPUP_MSG_ERROR.Hide(); }" />

                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="Popup_Campaign" ClientInstanceName="Popup_Campaign"
        ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="TopSides" HeaderText="Campaign" CloseAction="CloseButton"
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
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                <table width="100%">
                    <tr>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                            <asp:Label ID="Label21" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Search By" Width="70px"></asp:Label>
                        </td>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                            <asp:Label ID="Label32" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                Width="5px" Height="16px">:</asp:Label>
                        </td>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                            <asp:DropDownList ID="ddlSearchCampaign" runat="server" Width="150px" Height="24">
                                <asp:ListItem Value="CCP" Selected="True">Code</asp:ListItem>

                                <asp:ListItem Value="DCP">Description</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                        </td>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                            <asp:Label ID="Label33" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                Text="Text Search" Width="100px"></asp:Label>
                        </td>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                            <asp:Label ID="Label36" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                Width="5px" Height="16px">:</asp:Label>
                        </td>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                            <asp:TextBox ID="txtSearchCampaign" runat="server" Width="160px" Height="18"></asp:TextBox>
                        </td>
                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                            colspan="2">
                            <asp:Button ID="searchCampaignPopup" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_CAMPAIGN_POPUP"
                                CssClass="cursorPointer" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button12" runat="server" Text="Clear" Width="80px" Height="24px" CssClass="cursorPointer" OnClick="CLEAR_POPUP_CAMPAIGN" />
                        </td>
                    </tr>
                </table>

                <br />

                <div style="width: 100%; height: 450px; overflow: scroll">


                    <asp:GridView ID="gv_Campaign" runat="server" AutoGenerateColumns="False" BackColor="White"
                        PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                        AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="campaign_PageIndexChanging"
                        OnSelectedIndexChanging="campaign_SelectedIndexChanging">

                        <Columns>

                            <asp:BoundField DataField="C01CMP" HeaderText="Code" ReadOnly="True" SortExpression="C01CMP" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle Width="90px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="C01CNM" HeaderText="Description" ReadOnly="True" SortExpression="C01CNM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField></asp:BoundField>



                            <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <ItemStyle Width="70px" HorizontalAlign="Center" />
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
</asp:Content>
