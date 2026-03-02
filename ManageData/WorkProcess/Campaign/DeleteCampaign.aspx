<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="DeleteCampaign.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Campaign.DeleteCampaign" %>
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
        function dataListNote(dataTextss) {
            document.getElementById("dataListSelected").innerHTML = dataTextss;
        };

    </script>

    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Width="100%" Height="136px">
                    <ContentPaddings Padding="5px" />
                    <HeaderTemplate>
                        Delete Campaign
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <%-- Strat Contain --%>
                            <asp:HiddenField ID="ds_gvVendor" runat="server" />
                            <asp:HiddenField ID="ds_gvProductType" runat="server" />
                            <asp:HiddenField ID="ds_gvProductCode" runat="server" />
                            <asp:HiddenField ID="ds_gvModel" runat="server" />
                            <asp:HiddenField ID="ds_gvProductItem" runat="server" />
                            <asp:HiddenField ID="ds_gvCampaign" runat="server" />
                            <table width="100%">
                                <tr valign="top">
                                    <td width="55%" align="center">
                                        <table>
                                            <tr style="vertical-align: middle;">
                                                <td width="180px;">
                                                    <div style="width: 14px; height: 14px; margin-left: 3px; background: #0061C1; display: inline-block; border-radius: 3px;"></div>
                                                    New Campaign
                                                </td>
                                                <td width="150px">
                                                    <div style="width: 14px; height: 14px; background: #29998B; display: inline-block; border-radius: 3px;"></div>
                                                    Active Campaign
                                                </td>
                                                <td width="200px;">
                                                    <div style="width: 14px; height: 14px; background: #C11D1F; display: inline-block; border-radius: 3px;"></div>
                                                    End or Reject Campaign
                                                </td>
                                                <td width="150px;">
                                                    <div style="width: 14px; height: 14px; background: #9E9E9E; display: inline-block; border-radius: 3px;"></div>
                                                    Delete Campaign
                            
       
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>

                                        <div style="box-shadow: 2px 2px #AAAAAA; width: 700px; height: 870px; overflow-x: auto; overflow-y: auto; background-color: #F1F1F1; margin-top: 5px;">
                                            <div>
                                                <%-- <table width="1200" border="1" style="overflow: visible;" >
                                            <tr>
                                                <td>
                                                     Campaign Code
                                                </td>
                                                 <td>
                                                     Branch
                                                </td>
                                                 <td>
                                                     Type
                                                </td>
                                                 <td>
                                                     Name
                                                </td>
                                                 <td>
                                                     Start Date
                                                </td>
                                                 <td>
                                                     End Date
                                                </td>
                                                 <td>
                                                     CL Date
                                                </td>
                                                 <td>
                                                    STS
                                                </td>
                                                 <td>
                                                    Loan Type
                                                </td>

                                            </tr>    
                                            </table>--%>
                                            </div>
                                            <table width="1200" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="gv_listCampaign" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No records found" AutoGenerateColumns="false" OnRowDataBound="GV_LIST_CAMPAIGN_ROWDATABOUND" OnSelectedIndexChanging="GV_LIST_CAMPAIGN_SELECTED_INDEX_CHANGE" Width="99%">
                                                            <Columns>
                                                                <asp:BoundField DataField="C01CMP" HeaderText="Campaign Code" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="130"></HeaderStyle>

                                                                    <ItemStyle Width="130px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01BRN" HeaderText="Branch" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>

                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01CTY" HeaderText="Type" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>

                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01CNM" HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01SDT" HeaderText="Start Date" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>

                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01EDT" HeaderText="End Date" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>

                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01CLD" HeaderText="CL Date" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>

                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01CST" HeaderText="STS" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>

                                                                    <ItemStyle Width="50px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C01LTY" HeaderText="Loan Type" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="80"></HeaderStyle>

                                                                    <ItemStyle Width="80px"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="Select" />

                                                            </Columns>
                                                            <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                            <SelectedRowStyle BorderStyle="Groove" BorderColor="#666666" ForeColor="#000A0F" BorderWidth="2" Font-Bold="True" />

                                                        </asp:GridView>
                                                        <asp:HiddenField ID="ds_gvListCampaign" runat="server" />
                                                    </td>
                                                </tr>

                                            </table>



                                        </div>

                                    </td>
                                    <td width="600px">

                                        <table width="100%">
                                            <tr style="height: 20px;">
                                                <td align="right">
                                                    <asp:Label ID="txtTitleCampaign" runat="server"></asp:Label><asp:Label ID="lblCopyCampaign" runat="server" Style="margin-right: 4px;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset style="border-radius: 5px;">
                                                        <legend>Branch Filter</legend>
                                                        <asp:Panel runat="server" ID="box_branch_filter">
                                                            <table width="100%" border="0">
                                                                <tr valign="middle">
                                                                    <td width="150">
                                                                        <asp:RadioButton ID="rdbAllBranch" runat="server" Width="120" Text="All Branch" GroupName="[GroupBranch]"></asp:RadioButton>


                                                                    </td>
                                                                    <td width="150">
                                                                        <asp:RadioButton ID="rdbAllMybranch" runat="server" Width="120" Text="My Branch" GroupName="[GroupBranch]"></asp:RadioButton>

                                                                    </td>
                                                                    <td width="120"></td>
                                                                    <td align="center" width="150px">

                                                                        <dx:ASPxButton ID="btnMainSearch" OnClick="CLICK_MAIN_SEARCH" runat="server" Text="Find Campaign" Height="25" CssClass="imgNormal2" Cursor="pointer">
                                                                            <Image Url="~\Images\icon\find.png" Width="16"></Image>
                                                                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                            <DisabledStyle CssClass="imgGreyscal">
                                                                            </DisabledStyle>
                                                                        </dx:ASPxButton>

                                                                        <%--  <asp:ImageButton Height="18" ID="btnProductType" runat="server" AlternateText="ImageButton" ImageAlign="left" ImageUrl="~\Images\icon\eye.png" />&nbsp;Find Campaign --%>
                                                                    </td>
                                                                    <td width="120px"></td>
                                                                    <td align="center" width="150px">

                                                                        <dx:ASPxButton ID="btnMainDetail" OnClick="CLICK_MAIN_DETAIL" runat="server" Text="View All Detail" Height="25" CssClass="imgNormal2" Cursor="pointer">
                                                                            <Image Url="~\Images\icon\insert.png" Width="16"></Image>
                                                                            <DisabledStyle CssClass="imgGreyscal">
                                                                            </DisabledStyle>
                                                                        </dx:ASPxButton>

                                                                        <%-- <asp:ImageButton Height="16" ID="ImageButton1" runat="server" AlternateText="ImageButton" ImageAlign="left" ImageUrl="~\Images\icon\paper.png" />&nbsp;View All Detail--%>
                                                                    </td>
                                                                    <td width="40%"></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--    dsfsdfsdfsdfsf--%>


                                        <asp:Panel runat="server" ID="box_from_campaign">
                                            <table width="98%" border="0" style="margin-left: 5px; margin-top: 8px; border-radius: 5px; border: 1px solid #6C6C6C; background-color: #eff4f9;">
                                                <tr>
                                                    <td>

                                                        <table width="98%" border="0">

                                                            <tr>

                                                                <td rowspan="5" width="120px">

                                                                    <asp:CheckBoxList ID="chbCampaign" Width="120" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_CHB_CAMPAIGN">
                                                                        <asp:ListItem Value="1">From Campaign</asp:ListItem>
                                                                    </asp:CheckBoxList>

                                                                </td>

                                                                <td width="60" colspan="2" style="width: 60px;">Campaign
                                                                </td>
                                                                <td></td>
                                                                <td width="150" colspan="5">Campaign Name
                                                                </td>

                                                                <td></td>

                                                            </tr>
                                                            <tr>

                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="90" ID="campaignID" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_CAMPAIGN" AutoPostBack="true"></asp:TextBox>


                                                                </td>
                                                                <td width="20">
                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="serachCampaign" OnClick="CLICK_SEARCH_CAMPAIGN">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td></td>
                                                                <td colspan="5">
                                                                    <asp:TextBox runat="server" Height="20px" Width="259" ID="campaingName"></asp:TextBox>


                                                                </td>
                                                                <td></td>


                                                            </tr>
                                                            <tr>

                                                                <td colspan="7">
                                                                    <fieldset style="border-radius: 5px; height: 40px; width: 280px;">
                                                                        <legend>Campaign Status</legend>
                                                                        <table width="70%">
                                                                            <tr>

                                                                                <td>
                                                                                    <asp:RadioButtonList ID="campaignStatusCampaign" runat="server" RepeatDirection="Horizontal"
                                                                                        Width="270px">
                                                                                        <asp:ListItem Text="All Sts" Value="ALL" />
                                                                                        <asp:ListItem Text="New" Value="N" />
                                                                                        <asp:ListItem Text="Approve" Value="A" />
                                                                                        <asp:ListItem Text="End" Value="X" />
                                                                                    </asp:RadioButtonList>

                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>

                                                                <td></td>

                                                            </tr>
                                                            <tr>

                                                                <td style="height: 18px" colspan="2">Start date</td>
                                                                <td></td>
                                                                <td colspan="2" style="height: 18px">End date</td>
                                                                <td></td>
                                                                <td colspan="2" style="height: 18px">Closing Lay Bill date</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>

                                                                <td width="80px">

                                                                    <%--   <asp:TextBox ID="startDate" runat="server" Height="20px" Width="90px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate">
                                                                    </cc1:CalendarExtender>--%>
                                                                    <asp:TextBox ID="startDate" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    <%--      <dx:ASPxButton ID="calendarStart" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                        <Border BorderStyle="None" />
                                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                    </dx:ASPxButton>--%>
                                                                    <asp:ImageButton ID="calendarStart" runat="server" ImageAlign="Bottom"
                                                                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                </td>
                                                                <td></td>
                                                                <td width="70" align="left">
                                                                    <%--   <asp:TextBox ID="endDate" runat="server" Height="20px" Width="80px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarEnd" TargetControlID="endDate">
                                                                    </cc1:CalendarExtender>--%>
                                                                    <asp:TextBox ID="endDate" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarEnd" TargetControlID="endDate">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                                <td width="20">
                                                                    <%--       <dx:ASPxButton ID="calendarEnd" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                        <Border BorderStyle="None" />
                                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                    </dx:ASPxButton>--%>
                                                                    <asp:ImageButton ID="calendarEnd" runat="server" ImageAlign="Bottom"
                                                                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" Width="23px" />
                                                                </td>
                                                                <td width="20px"></td>
                                                                <td align="left" width="70px">
                                                                    <%--<asp:TextBox ID="closingDate" runat="server" Height="20px" Width="80px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarClosing" TargetControlID="closingDate">
                                                                    </cc1:CalendarExtender>--%>
                                                                    <asp:TextBox ID="closingDate" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarClosing" TargetControlID="closingDate">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    <%--     <dx:ASPxButton ID="calendarClosing" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                        <Border BorderStyle="None" />
                                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                    </dx:ASPxButton>--%>
                                                                    <asp:ImageButton ID="calendarClosing" runat="server" ImageAlign="Bottom"
                                                                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                </td>
                                                                <td></td>

                                                            </tr>
                                                        </table>


                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="box_from_product">
                                            <table width="98%" border="0" style="margin-left: 5px; margin-top: 8px; border-radius: 5px; border: 1px solid #6C6C6C; background-color: #eff4f9;">

                                                <tr>
                                                    <td>

                                                        <table width="100%" border="0">

                                                            <tr>
                                                                <td rowspan="10" width="120px">
                                                                    <asp:CheckBoxList ID="chbProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_CHB_PRODUCT">
                                                                        <asp:ListItem Value="1">From Product</asp:ListItem>
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td width="90px">Product Type 
                                                                </td>
                                                                <td width="50" align="left"></td>
                                                                <td>Type Name
                                                                </td>
                                                                <td></td>




                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="90" Height="20px" ID="productType" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_PRODUCT_TYPE" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td width="50" align="left">

                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="serachProductType" OnClick="CLICK_SELECT_PRODUCT_TYPE">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>


                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="260px" ID="typeName"></asp:TextBox>

                                                                </td>


                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>

                                                                <td width="90px">Product Code 
                                                                </td>
                                                                <td width="50" align="left"></td>
                                                                <td>Code Name
                                                                </td>

                                                                <td></td>

                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="90" Height="20px" ID="productCode" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_PRODUCT_CODE" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td width="50" align="left">

                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchProductCode" OnClick="CLICK_SELECT_PRODUCT_CODE">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>


                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="260px" ID="codeName"></asp:TextBox>

                                                                </td>
                                                                <td></td>

                                                            </tr>

                                                            <tr>
                                                                <td>Brand </td>
                                                                <td width="50" align="left"></td>
                                                                <td>Brand Name </td>
                                                                <td></td>


                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="90" ID="brandTxt" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_BRAND" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td width="50" align="left">
                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchBranchTxt" OnClick="CLICK_SELECT_BRAND">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="260px" ID="brandName"></asp:TextBox>
                                                                </td>
                                                                <td>&nbsp;</td>


                                                            </tr>
                                                            <tr>

                                                                <td width="90px">Model 
                                                                </td>
                                                                <td width="50" align="left"></td>
                                                                <td>Model Name
                                                                </td>

                                                                <td></td>

                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="90" Height="20px" ID="modelTxt" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_MODEL" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td width="50" align="left">

                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchModel" OnClick="CLICK_SELECT_MODEL">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />

                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>


                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="260px" ID="modelName"></asp:TextBox>

                                                                </td>
                                                                <td></td>

                                                            </tr>
                                                            <tr>
                                                                <td>Product Item</td>
                                                                <td width="50" align="left"></td>
                                                                <td>Product Item Desciption </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="90" ID="productItem" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_PRODUCT_ITEM" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td width="50" align="left">
                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchProductItem" OnClick="CLICK_SELECT_PRODUCT_ITEM">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="260px" ID="productItemDes"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>


                                                        </table>

                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="box_from_vendor">
                                            <table width="98%" border="0" style="margin-left: 5px; margin-top: 8px; border-radius: 5px; border: 1px solid #6C6C6C; background-color: #eff4f9;">
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" style="table-layout: fixed;">

                                                            <tr>
                                                                <td rowspan="6" width="120px">
                                                                    <asp:CheckBoxList ID="chbVendor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_CHB_VENDOR">
                                                                        <asp:ListItem Value="1">From Vendor</asp:ListItem>
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                                <td width="98" style="height: 20px">Vendor Code</td>
                                                                <td width="50" style="height: 20px"></td>

                                                                <td width="110" style="height: 20px">Vendor Name
                                                                </td>
                                                                <td width="30"></td>
                                                                <td width="60" style="height: 20px">% O/D 
                                                                </td>



                                                                <td width="70" style="height: 20px">% W/O</td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="90" ID="vendorCode" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_VENDOR" AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                                <td width="50">
                                                                    <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchVendor" OnClick="CLICK_SEARCH_VENDOR">
                                                                        <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                        <Border BorderStyle="None"></Border>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                        <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>

                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="110px" ID="vendorName"></asp:TextBox>

                                                                </td>
                                                                <td></td>
                                                                <td width="38">
                                                                    <asp:TextBox runat="server" Height="20px" Width="50px" ID="odTxt" OnChange="JavaScript:chkNum3(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>

                                                                </td>

                                                                <td width="38">
                                                                    <asp:TextBox runat="server" Height="20px" Width="50px" ID="wqTxt" OnChange="JavaScript:chkNum3(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>

                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Old Vendor Code
                                                                </td>

                                                                <td colspan="5" rowspan="2">
                                                                    <fieldset style="border-radius: 5px; width: 265px; margin-left: 24px; height: 40px;">
                                                                        <legend>Campaign Status</legend>
                                                                        <table width="100%">
                                                                            <tr>

                                                                                <td>
                                                                                    <asp:RadioButtonList ID="CampaigStatus"  runat="server" RepeatDirection="Horizontal"
                                                                                        Width="270px">
                                                                                        <asp:ListItem Text="All Sts" Value="ALL" />
                                                                                        <asp:ListItem Text="New" Value="N" />
                                                                                        <asp:ListItem Text="Approve" Value="A" />
                                                                                        <asp:ListItem Text="End" Value="E" />
                                                                                    </asp:RadioButtonList>

                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Height="20px" Width="90" ID="oldVendorCode"></asp:TextBox>
                                                                </td>


                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Rank
                                                                </td>
                                                                <td></td>
                                                                <td colspan="2">Start Date Rank</td>
                                                                <td colspan="2">End Date Rank</td>
                                                                <td></td>

                                                            </tr>
                                                            <tr>
                                                                <td>

                                                                    <asp:DropDownList ID="ddlRank" runat="server" Height="26" Width="95px" AppendDataBoundItems="true">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td></td>
                                                                <td colspan="2" align="left" valign="middle">
                                                                    <%--    <asp:TextBox ID="startDateRank" runat="server" Height="20px" Width="90px"></asp:TextBox>--%>
                                                                    <asp:TextBox ID="startDateRank" runat="server" Width="88px" Height="20px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarStartRank" TargetControlID="startDateRank">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:ImageButton ID="calendarStartRank" runat="server" ImageAlign="Bottom"
                                                                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" Width="23px" Height="21px" />
                                                                </td>

                                                                <td colspan="2" align="left">
                                                                    <%--<asp:TextBox ID="endDateRank" runat="server" Height="20px" Width="90px"></asp:TextBox>--%>
                                                                    <asp:TextBox ID="endDateRank" runat="server" Width="88px" Height="20px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True"
                                                                        Format="dd/MM/eeee" PopupButtonID="calendarEndRank" TargetControlID="endDateRank">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:ImageButton ID="calendarEndRank" runat="server" ImageAlign="Bottom"
                                                                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" Width="23px" Height="21px" />
                                                                </td>




                                                                <td></td>

                                                            </tr>
                                                            <tr>
                                                                <td colspan="8"></td>
                                                            </tr>



                                                        </table>


                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="box_note">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <fieldset style="border-radius: 5px; background-color: #eff4f9;">
                                                            <legend>Note</legend>
                                                            <table width="100%" style="align-content: center">
                                                                <tr valign="middle">
                                                                    <td align="left">
                                                                        <div id="dataListSelected" style="width: 520px; height: 150px; background-color: #FFFFFF; overflow-y: scroll"></div>

                                                                    </td>
                                                                    <td width="30" align="center" valign="top">
                                                                        <div>
                                                                            <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" CssClass="button_bg_sky" ID="searchNote">
                                                                                <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                <Border BorderStyle="None"></Border>

                                                                                <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                        <div style="margin-top: 10px;">
                                                                            <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" CssClass="button_bg_sky" ID="closeNote">
                                                                                <Image Width="14px" Url="~\Images\icon\close.png"></Image>

                                                                                <Border BorderStyle="None"></Border>

                                                                                <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>

                            <%--  End Contain--%>
                            <%--   Popup Contain --%>
                            <dx:ASPxPopupControl ID="popupAlert" ClientInstanceName="popupAlert"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" SupportsDisabledAttribute="True">
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
                                                    <dx:ASPxButton ID="btnPopupAler" runat="server" Text="OK" Width="100px">

                                                        <ClientSideEvents Click="function(s, e) { popupAlert.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAlertApp" ClientInstanceName="PopupAlertApp"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <dx:ASPxLabel ID="lblMsgAlertApp" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertApp.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
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
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server"
                                ClientInstanceName="LoadingPanel" Height="170px" Width="320px">
                            </dx1:ASPxLoadingPanel>
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
                                                    <asp:Label ID="Label24" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label25" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label26" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label27" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchCampaign" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchCampaignPopup" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_CAMPAIGN_POPUP"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearCampaignPopup" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_CAMPAIGN" />
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

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="C01CNM" HeaderText="Description" ReadOnly="True" SortExpression="C01CNM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        
                                                    </asp:BoundField>
                                                    



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
                            <dx:ASPxPopupControl ID="Popup_ProductType" ClientInstanceName="Popup_ProductType"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Product Type" CloseAction="CloseButton"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label10" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label13" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchProducttype" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CPT" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DPT">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label14" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label15" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProducttype" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="productTypeSearch" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_TYPE_POPUP"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="productTypeClear" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_PRODUCTTYPE" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_ProductType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="productType_PageIndexChanging"
                                                OnSelectedIndexChanging="productType_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="T40TYP" HeaderText="Code" ReadOnly="True" SortExpression="T40TYP" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="T40DES" HeaderText="Description" ReadOnly="True" SortExpression="T40DES" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                      
                                                    </asp:BoundField>
                                                   



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

                            <dx:ASPxPopupControl ID="Popup_ProductCode" ClientInstanceName="Popup_ProductCode"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Product Code" CloseAction="CloseButton"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label16" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label20" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchProductCode" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CPC" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DPC">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label21" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label22" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProductCode" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="productCodeSearch" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_CODE"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="productCodeClear" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_PRODUCTCODE" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_ProductCode" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="productCode_PageIndexChanging"
                                                OnSelectedIndexChanging="productCode_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="T41TYP" HeaderText="Type" ReadOnly="True" SortExpression="P10VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="90px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="T41COD" HeaderText="Code" ReadOnly="True" SortExpression="T41COD" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41DES" HeaderText="Description" ReadOnly="True" SortExpression="T41DES" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        
                                                    </asp:BoundField>
                                                   



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
                            <dx:ASPxPopupControl ID="Popup_Brand" ClientInstanceName="Popup_ProductCode"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Brand" CloseAction="CloseButton"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label2" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchBrands" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CB" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DB">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label3" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label4" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchBrands" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchingBrand" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_BRAND_POPUP"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearBrand" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_BRAND" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">
                                            <asp:GridView ID="gv_Brand" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="brand_PageIndexChanging"
                                                OnSelectedIndexChanging="brand_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="T42BRD" HeaderText="Code" ReadOnly="True" SortExpression="T42BRD" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="T42DES" HeaderText="Description" ReadOnly="True" SortExpression="T42DES" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                      
                                                    </asp:BoundField>
                                                   



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
                            <dx:ASPxPopupControl ID="Popup_Model" ClientInstanceName="Popup_Model"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Model" CloseAction="CloseButton"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchModel" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CMD" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DMD">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label8" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label9" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchModel" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchModelpopup" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_MODEL_POPUP"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearModelpopup" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_MODEL" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_Model" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="model_PageIndexChanging"
                                                OnSelectedIndexChanging="model_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="T43MDL" HeaderText="Code" ReadOnly="True" SortExpression="T43MDL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="T43DES" HeaderText="Description" ReadOnly="True" SortExpression="T43DES" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                       
                                                    </asp:BoundField>
                                                    



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
                            <dx:ASPxPopupControl ID="Popup_ProductItem" ClientInstanceName="Popup_ProductItem"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Product Item" CloseAction="CloseButton"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label17" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label18" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchProductItem" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CPI" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DPI">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label19" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label23" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProductItem" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="productItemSearch" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_ITEM_POPUP"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="productItemClear" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_PRODUCT_ITEM" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_productItem" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="productItem_PageIndexChanging"
                                                OnSelectedIndexChanging="productItem_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="T44ITM" HeaderText="Code" ReadOnly="True" SortExpression="T44ITM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="T44DES" HeaderText="Description" ReadOnly="True" SortExpression="T44DES" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                     
                                                    </asp:BoundField>
                                                   



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
                            <dx:ASPxPopupControl ID="Popup_Vendor" ClientInstanceName="Popup_Vendor"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Vendor" CloseAction="CloseButton"
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
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label11" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchVendor" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CV" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DV">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="lbl_SelectProductType" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label12" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchVendor" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Popup_searchVendor" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_VENDOR_POPUP"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Popup_clearVendor" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_VENDOR" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="gvVendor_PageIndexChanging"
                                                OnSelectedIndexChanging="gvVendor_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="P10VEN" HeaderText="Code" ReadOnly="True" SortExpression="P10VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="P10NAM" HeaderText="Description" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        
                                                    </asp:BoundField>
                                            
                                                    <asp:BoundField DataField="T71DES" Visible="false"></asp:BoundField>
                                                    <asp:BoundField DataField="P10CPD" Visible="false"></asp:BoundField>
                                                    <asp:BoundField DataField="P10PTY" Visible="false"></asp:BoundField>


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
                            <%--  End Popup Contain --%>

                            <br />
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
</asp:Content>