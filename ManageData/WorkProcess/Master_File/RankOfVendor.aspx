<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="RankOfVendor.aspx.cs" Inherits="ManageData_WorkProcess_RankOfVendor" %>
<%@ Register Src="~/ManageData/WorkProcess/UserControl/UC_Judgment.ascx" TagName="UC_Judgment" TagPrefix="uc1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phContents" runat="Server">

    <script type="text/javascript" src="../../../Js/ValidateNumber.js"></script>
    <script type="text/javascript">

        function selectAll(chkAll) {
            var gvrecipe = document.getElementById('<%=GridViewCheckbox2.ClientID %>');
            var inputs = gvrecipe.getElementsByTagName("input");

            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].id.indexOf("CheckBoxInsert") != -1)
                    inputs[i].checked = chkAll.checked;

            }
        }
<%--        document.documentElement.onclick = function () {
            $find("<%= CalendarStartDate.ClientID %>").hide();
        }--%>
    </script>

    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    GroupBoxCaptionOffsetY="-28px"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100%"
                    Height="136px">
                    <ContentPaddings Padding="5px" />

                    <HeaderTemplate>
                        Rank of Vendor
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <br />
                            <asp:HiddenField ID="dt_vendor_detail" runat="server" />
                            <asp:HiddenField ID="ds_Hiddengrid" runat="server" />
                            <asp:HiddenField ID="ds_popup" runat="server" />
                            <asp:HiddenField ID="ds_grid_rv_New" runat="server" />
                            <table width="100%" border="0" style="border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; table-layout:fixed;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                        <asp:Label ID="lbl_RankOfVendor" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Add Rank of Vendor" Width="135px"></asp:Label>
                                    </td>
                                    

                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 170px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 180px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 100px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 140px;"></td>
                                    <td style="width:500px;"></td>
                                    <td></td>

                                </tr>
                                <tr style="height: 30px;">
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                        <asp:Label ID="Label3" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Vendor Code" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 130px;">
                                        <asp:TextBox runat="server" ID="txtvendorCode" Height="20px" Width="130px" Style="margin-left: 3px;"></asp:TextBox>

                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 160px;">
                                        <asp:TextBox runat="server" ID="txtvendorName" Height="20px" Width="130px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px;">
                                        <asp:Label ID="Label9" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Start Date" Width="90px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 130px;">
                                        <asp:TextBox ID="startDate" runat="server" Height="20" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                            Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 30px;">

                                        <%--  <dx:ASPxButton ID="calendarStart" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                            <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                            <Border BorderStyle="None" />
                                            <DisabledStyle CssClass="calendars"></DisabledStyle>
                                        </dx:ASPxButton>--%>
                                        <asp:ImageButton ID="calendarStart" runat="server" ImageAlign="Bottom"
                                            ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                    </td>
                                    <td style="width: auto;"></td>
                                </tr>
                                <tr style="height: 30px;">
                                    <td style="text-align: left; font-family: Tahoma; font-size: small;">
                                        <asp:Label ID="lbRankCode" runat="server" Height="16px"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Rank Code" Width="80px"></asp:Label>

                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 110px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRank" MaxLength="2" CssClass="txtUppers" Height="20px" Width="130px" autocomplete="off"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="searchRank" runat="server" OnClick="btAddRank_OnClick" BackColor="#EFF4F9" Cursor="pointer" EnableDefaultAppearance="False" ToolTip="Searching Rank Code" CssClass="button_bg_sky" AutoPostBack="false">
                                                        <Image Url="~\Images\icon\search.png" Width="18"></Image>

                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                    <%--   <td style="text-align: left; font-family: Tahoma; padding-left: 5px; font-size: small; height: 24px;" width="200">
                                        
                                    </td>--%>
                                    <td></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                        <asp:Label ID="Label13" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="End Date" Width="114px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;">
                                        <asp:TextBox ID="setEndDate" runat="server" Enabled="false" value="99/99/9999" Width="130" Height="21" ReadOnly="false"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center" style="height: 40px; padding-bottom: 10px;">
                                        <br />
                                        <asp:Timer ID="Timer1" runat="server" OnTick="TimerTick" Interval="3000"></asp:Timer>
                                        <div id="imgdivLoading" align="center" valign="middle" runat="server" style="width: 95%; padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;">
                                            <asp:Image ID="LoadImg" runat="server" ImageUrl="~/Images/loading2.gif" BackColor="Transparent" ImageAlign="AbsMiddle" />
                                        </div>
                                        <dx:ASPxButton ID="btnAddData" runat="server" Text="Add" CssClass="AlignButtonCenter" OnClick="btAdd_Click">
                                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />

                                        </dx:ASPxButton>


                                        <dx:ASPxButton ID="btnClearData" CssClass="AlignButtonCenter" runat="server" Text="Clear" Width="70px" OnClick="set_clear_add">
                                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="hiddenRankCode" Visible="false" Height="20px" Width="130px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 130px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 130px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 90px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 130px;"></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 30px;"></td>
                                    <td style="width: auto;"></td>
                                </tr>

                            </table>
                            <br />
                            <dx:ASPxPageControl Width="100%" Height="100%" ID="tabDetail" runat="server" ActiveTabIndex="0" OnActiveTabChanged="ACTIVE_TAB_CHANGE" EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                ActivateTabPageAction="Click" LoadingPanelImagePosition="Top" AutoPostBack="true" LoadingPanelImage-Url="~/Images/loading2.gif">
                                <TabPages>

                                    <dx:TabPage Text="Insert" Name="Inserts">
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControlStep1" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%">
                                                    <tr align="center">
                                                        <td>
                                                            <table width="90%">
                                                                <tr>
                                                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 45%; height: 400px;" align="left">
                                                                        <table style="margin-left: -3px;">
                                                                            <tr>
                                                                                <td>
                                                                                    <%--<asp:TextBox runat="server" Height="21px" Width="122px" ID="txtSearchVendor" OnTextChanged="SELECT_VENDOR_ONCHANGE" autocomplete="off" AutoPostBack="true" MaxLength="13" OnKeyPress="return chkNumber(this)"></asp:TextBox>--%>
                                                                                    <dx:ASPxTextBox ID="txtSearchVendor" runat="server" Width="125px" Height="21px" AutoPostBack="true" MaxLength="13" OnKeyPress="return chkNumber(this)" OnTextChanged="SELECT_VENDOR_ONCHANGE">
                                                                                        <ClientSideEvents KeyDown="function(s, e) { if(e.htmlEvent.keyCode == 13) { LoadingPanel.Show(); } }" />
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td align="right" height="25">
                                                                                    <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" CssClass="imgNormal" Cursor="pointer" OnClick="CLEAR_DATA_LIST">
                                                                                        <Image Url="~\Images\icon\refresh.png" Width="16"></Image>
                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                                        </DisabledStyle>
                                                                                    </dx:ASPxButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <%--<asp:UpdateProgress ID="upProgGVCheckbox1" runat="server">
                                                                            <ProgressTemplate>--%>
                                                                        <%--<div id="imgdivLoading1" align="center" valign="middle" runat="server" style=" width: 85%;padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;">
                                                                                <asp:Image ID="LoadImg1" runat="server" ImageUrl="~/Images/loading2.gif" BackColor="Transparent" ImageAlign="AbsMiddle"/>                                               
                                                                                </div>--%>
                                                                        <%--</ProgressTemplate>
                                                                        </asp:UpdateProgress>--%>
                                                                        <%--<asp:UpdatePanel ID="upGVCheckbox1" runat="server">
                                                                            <ContentTemplate>--%>
                                                                        <div style="width: 100%; height: auto; min-height: 402px; border: 0.05em solid #808080; background-color: #FFFFFF;">
                                                                            <asp:GridView ID="GridViewCheckbox1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                AllowPaging="True" PageSize="12" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                ShowHeaderWhenEmpty="true" DataKeyNames="P10VEN,P10NAM" OnPageIndexChanging="gv1_PageIndexChanging">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="cbSelectAll1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxSelectAllProductTypeChanged" />
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10"></HeaderStyle>
                                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="CheckBoxInsert1" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="P10VEN" HeaderText="Vendor Code" SortExpression="P10VEN">
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
                                                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="P10NAM" HeaderText="Vendor Name"></asp:BoundField>
                                                                                </Columns>
                                                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                <AlternatingRowStyle BackColor="White" />
                                                                                <FooterStyle BackColor="#CCCC99" />
                                                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                                                <RowStyle BackColor="#F7F7DE" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                        <%--</ContentTemplate>
                                                                        </asp:UpdatePanel>--%>
                                                                    </td>
                                                                    <td style="text-align: center; font-family: Tahoma; font-size: small; width: 100px;">
                                                                        <table width="90%">
                                                                            <tr align="center">
                                                                                <td style="width: 200px">
                                                                                    <table>
                                                                                        <tr style="width: 150px" align="center">
                                                                                            <td>
                                                                                                <dx:ASPxButton ID="btnAdd" CssClass="AlignButtonCenter" runat="server" OnClick="BTN_TRANSFER_CLICK">
                                                                                                    <Image Url="~\Images\icon\front.png" Width="16"></Image>

                                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                                    <Border BorderStyle="Dotted" />
                                                                                                </dx:ASPxButton>
                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 150px" align="center">
                                                                                                <dx:ASPxButton ID="btnBack" CssClass="AlignButtonCenter" runat="server" OnClick="BTN_BACK_CLICK" ImagePosition="Top">
                                                                                                    <Image Url="~\Images\icon\back.png" Width="16"></Image>

                                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                                    <Border BorderStyle="Dotted" />
                                                                                                </dx:ASPxButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>
                                                                    <td style="text-align: right; font-family: Tahoma; font-size: small; width: 45%; height: 400px;" align="right" valign="top">
                                                                        <table style="text-align: right;" width="90%">
                                                                            <tr>

                                                                                <td align="right" height="25">
                                                                                  <%--  <dx:ASPxButton Visible="false" ID="deleteListData" runat="server" Text="Delete" Cursor="pointer" OnClick="DELETE_DATA_LIST_CONFIRM">
                                                                                        <Image Url="~\Images\icon\click_trash.png" Width="16"></Image>
                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                                        </DisabledStyle>
                                                                                    </dx:ASPxButton>--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div style="width: 100%; height: 405px; border: 0.05em solid #808080; background-color: #FFFFFF;">
                                                                            <asp:GridView ID="GridViewCheckbox2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" EmptyDataText="NO ITEMS FOUND !"
                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                ShowHeaderWhenEmpty="true">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="cbSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxSelectAllProductTypeSelectChanged" />
                                                                                        </HeaderTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="10"></HeaderStyle>
                                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="CheckBoxInsert" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Vendor Code" DataField="P10VEN">
                                                                                        <HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
                                                                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField HeaderText="Vendor Name" DataField="P10NAM" />
                                                                                </Columns>
                                                                                <EmptyDataRowStyle BackColor="#F7F7DE" ForeColor="#FD4D00" HorizontalAlign="center" />
                                                                                <AlternatingRowStyle BackColor="White" />
                                                                                <FooterStyle BackColor="#CCCC99" />
                                                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                                                <RowStyle BackColor="#F7F7DE" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Modify and Delete" Name="Modify">
                                        <TabStyle Font-Bold="True">
                                        </TabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControlStep2" runat="server" SupportsDisabledAttribute="True">
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                            <asp:Label ID="lbl_SearchBy" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                Text="Search By" Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                            <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                                Width="5px" Height="16px">:</asp:Label>
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 250px; height: 24px;">
                                                            <asp:DropDownList ID="ddl_SearchBy" runat="server" Height="25" Width="200px">
                                                                <asp:ListItem Value="VC" Selected="True">Vendor Code</asp:ListItem>
                                                                <asp:ListItem Value="VN">Vendor Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;&nbsp;
                                                        </td>


                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                                            <asp:TextBox ID="txt_Brand" runat="server" Height="19px" Width="200px"></asp:TextBox>&nbsp;&nbsp;
                                                        </td>

                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                            <dx:ASPxButton ID="btn_search" runat="server" Text="Search" Width="70px" OnClick="btn_search_Click">
                                                            </dx:ASPxButton>
                                                        </td>

                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px; height: 24px;"></td>
                                                    </tr>

                                                </table>
                                                <br />
                                                <table width="100%">
                                                    <tr>
                                                        <td height="400" valign="top">
                                                            <div style="width: auto; margin-top: -15px;">
                                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView2_PageIndexChanging"
                                                                    OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanging="GridView2_SelectedIndexChanging">
                                                                    <Columns>

                                                                        <asp:BoundField DataField="P16VEN" HeaderText="Vendor Code" ReadOnly="True" SortExpression="P16VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="P10NAM" HeaderText="Vendor Name" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="P16RNK" HeaderText="Vendor Rank" ReadOnly="True" SortExpression="P16RNK" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="P16STD" HeaderText="Start Date" ReadOnly="True" SortExpression="P16STD" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                        </asp:BoundField>

                                                                        <asp:BoundField DataField="P16END" HeaderText="End Date" ReadOnly="True" SortExpression="P16END" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                        </asp:BoundField>


                                                                        <asp:BoundField DataField="P16ENDS" HeaderText="End Date" ReadOnly="True" Visible="false" SortExpression="P16ENDS" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                        </asp:BoundField>


                                                                        <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                        PageButtonCount="4" />
                                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <RowStyle BackColor="#F7F7DE" />
                                                                </asp:GridView>
                                                            </div>


                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtSqlAll" runat="server" Width="250px" Visible="false"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                                <br />

                                            </dx:ContentControl>
                                        </ContentCollection>

                                    </dx:TabPage>
                                </TabPages>

                                <LoadingPanelImage Url="~/Images/loading2.gif"></LoadingPanelImage>
                            </dx:ASPxPageControl>
                            <br />


                            <dx:ASPxPopupControl ID="Popup_AddRank" ClientInstanceName="Popup_AddRank"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Rank Code" CloseAction="CloseButton"
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
                                                    <asp:DropDownList ID="ddl_popup_SearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="RC" Selected="True">Ranking</asp:ListItem>

                                                        <asp:ListItem Value="RD">Addition TCL(%)</asp:ListItem>
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
                                                    <asp:TextBox ID="txt_Detail" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <dx:ASPxButton ID="btn_popup_search" runat="server" Text="Search" CssClass="AlignButtonCenter" Width="80px" Height="24px" OnClick="btn_popup_search_Click">
                                                    </dx:ASPxButton>

                                                    <dx:ASPxButton ID="btn_popup_clear" runat="server" Text="Clear" CssClass="AlignButtonCenter" Width="80px" Height="24px" OnClick="btn_popup_clear_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px;">


                                            <asp:GridView ID="gvRank" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px"
                                                EnableModelValidation="True" AllowPaging="True" PageSize="21" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                EmptyDataText="No records found" OnSelectedIndexChanging="gvRank_SelectedIndexChanging" OnPageIndexChanging="gvRank_PageIndexChanging">
                                                <Columns>


                                                    <asp:BoundField DataField="T12RNK" HeaderText="Ranking" ReadOnly="True" SortExpression="T12RNK" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T12RTE" HeaderText="Additional TCL(%)" ReadOnly="True" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="T12RTE">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AT12STD" HeaderText="Start Date" ReadOnly="True" SortExpression="T12STD" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T12END" HeaderText="End Date" ReadOnly="True" SortExpression="T12END" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="" ReadOnly="True" SortExpression="">
                                                        <ItemStyle />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
                                Height="150px" Width="300px">
                            </dx1:ASPxLoadingPanel>
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
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/icon/question.png" />

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
                                                    <dx:ASPxButton ID="btnClickOk" runat="server" value="OK" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertApp.Hide();}" />

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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/success.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsg" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #1F6E42;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnOK" runat="server" Text="OK" Width="100px" OnClick="btnOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupMsgSuccess" ClientInstanceName="PopupMsgSuccess" ShowPageScrollbarWhenModal="true"
                                HeaderText="Success" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icon/success.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsgSuccess" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #1F6E42;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnSuccess" runat="server" Text="OK" Width="100px" OnClick="set_clear_add">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }" />

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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/deletenew.png" />
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
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnConfirm_OK" runat="server" Text="OK" Width="100px" OnClick="btnConfirm_OK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirm_Cancel" runat="server" Text="Cancel" Width="100px"
                                                        OnClick="btnConfirm_Cancel_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupConfirmSave" ClientInstanceName="PopupConfirmSave"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Tahoma" Font-Size="Small"
                                                        Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="ASPxbtnTransfer" runat="server" Text="OK" Width="100px" OnClick="btnConfirm_Add_OK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="ASPxbtnBack" runat="server" Text="Cancel" Width="100px"
                                                        OnClick="btnConfirm_Add_Cancel_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupConfirmDeleteVendor" ClientInstanceName="PopupConfirmDeleteVendor"
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/icon/deletenew.png" /><br />
                                                    <br />
                                                    <b style="color: darkred;">Do you really want to delete vendor data ?</b>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="confirmVendorDelete" runat="server" value="DeleteVendor" Text="OK" Width="100px" OnClick="DELETE_DATA_LIST">

                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteVendor.Hide(); LoadingPanel.Show(); }"></ClientSideEvents>

                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="cancelMainDelete" runat="server" Text="Cancel" Width="100px">

                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDeleteVendor.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAlerCheckDateBetween" ClientInstanceName="PopupAlerCheckDateBetween" ShowPageScrollbarWhenModal="true"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="txtAlertCheckDateBetween" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="btnCheckDateBetween" runat="server" Text="OK" Width="100px" AutoPostBack="false" OnClick="CLEAR_DATA_LIST">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlerCheckDateBetween.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAlertMsgError" ClientInstanceName="PopupAlertMsgError" ShowPageScrollbarWhenModal="true"
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
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="txtAlertError" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="okError" runat="server" Text="OK" Width="100px" OnClick="BTN_OK_VALIDATION">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertMsgError.Hide(); }" />

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