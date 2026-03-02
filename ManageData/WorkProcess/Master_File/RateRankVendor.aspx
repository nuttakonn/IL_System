<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="RateRankVendor.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Master_File.RateRankVendor" %>
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


    <script type="text/javascript" src="../../../Js/shortcut.js"></script>
    <script type="text/javascript" src="../../../Js/ValidateNumber.js"></script>
    <script type="text/javascript">

        <%--document.documentElement.onclick = function () {
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
                        Rate of rank vendor
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">                         
                            <br />
                             <asp:HiddenField ID="ds_Hiddengrid" runat="server" />
                             <asp:HiddenField ID="ds_grid_rv" runat="server" />
                             <asp:HiddenField ID="ds_grid_rv_New" runat="server" />
                            <table width="100%" border="0" style="border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9;">
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 350px; height: 24px;" colspan="12">
                                        <asp:Label ID="lbl_addRank" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Add Ranking" Width="300px"></asp:Label>
                                    </td>


                                </tr>
                                <tr style="height: 30px;">
                                    <td style="text-align: left; font-family: Tahoma; font-size: small;">
                                        <asp:Label ID="lbRankCode" runat="server" Height="16px"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Rank Code" Width="80px"></asp:Label>

                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 90px;">
                                        <asp:TextBox runat="server" MaxLength="2" CssClass="txtUppers" ID="txtRank" Height="20px" Width="130px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; padding-left: 5px; font-size: small; height: 24px;" width="40">
                                    </td>

                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px; width: 80px;">
                                        <asp:Label ID="Label13" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Start Date" Width="80px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td width="95" valign="middle">

                                        <asp:TextBox ID="startDate" runat="server" Height="21px" Width="130px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarStartDate" runat="server"
                                            Format="dd/MM/eeee" PopupButtonID="setStartDate" TargetControlID="startDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td width="30">
                                   <%--     <dx:ASPxButton ID="setStartDate" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                            <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                            <Border BorderStyle="None" />
                                            <DisabledStyle CssClass="calendars"></DisabledStyle>
                                        </dx:ASPxButton>--%>
                                           <asp:ImageButton ID="setStartDate" runat="server" ImageAlign="Bottom" 
                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                        <asp:Label ID="Label2" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="BOT Regulation" Width="110px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;" width="200">
                                        <asp:DropDownList ID="botRegulation" runat="server" Height="25" Width="200px">
                                            <asp:ListItem Value="N" Selected="True">N: Not over 5 times of salary (Old Condition)</asp:ListItem>
                                            <asp:ListItem Value="Y">Y: Over 5 times of salary</asp:ListItem>

                                        </asp:DropDownList>


                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <asp:Label ID="Label3" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Percent of TCL" Width="100px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;" width="135">
                                        <asp:TextBox runat="server" Height="20px" Width="130px" ID="txtTCL" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>


                                    </td>
                                    <td>&nbsp;%
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 80px;">
                                        <asp:Label ID="Label9" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="End Date" Width="80px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px; height: 24px;">
                                        <asp:TextBox ID="setEndDate" runat="server" Enabled="false" value="99/99/9999" Width="130" Height="21" ReadOnly="false"></asp:TextBox>

                                    </td>
                                    <td></td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                        <asp:Label ID="Label5" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Up-Rank Condition" Width="130px" Height="16px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; height: 24px;" width="150">
                                        <asp:DropDownList ID="ddlupRank" runat="server" Height="25" Width="200px">
                                            <asp:ListItem Value="N" Selected="True">N: Up-Rank incase not have contract active (Old Condition)</asp:ListItem>
                                            <asp:ListItem Value="Y">Y: Up-Rank all case - outstanding balance</asp:ListItem>

                                        </asp:DropDownList>

                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>


                                <tr>
                                    <td colspan="5" align="right" valign="middle">
                                        <dx:ASPxButton ID="btnAdd" runat="server" Width="70px" CssClass="AlignButtonCenter" Text=" Add " CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                            CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                            OnClick="btnAdd_Click">
                                            <ClientSideEvents Click="function(s, e) { 
                                                                       LoadingPanel.Show();
                                                                     }" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td></td>
                                    <td colspan="10" align="left" style="height: 40px;" valign="middle">



                                        <dx:ASPxButton ID="btnClearData" runat="server" Text="Clear" Width="70px" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                            CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" OnClick="btnClearData_Click">
                                        </dx:ASPxButton>


                                    </td>


                                </tr>

                            </table>
                            <br />
                            <hr />
                            <br />

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
                                            <asp:ListItem Value="RK" Selected="True">Ranking</asp:ListItem>
                                            <asp:ListItem Value="TCL">% of TCL</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">

                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                        <asp:TextBox ID="txt_Brand" runat="server" Height="19px" Width="200px"></asp:TextBox>&nbsp;&nbsp;
                                    </td>

                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btn_search" runat="server" Text="Search" Width="70px" OnClick="btn_search_Click">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <dx:ASPxButton ID="btn_clear" runat="server" Text="Clear" Width="70px" OnClick="btn_clear_Click">
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 600px; height: 24px;"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="E_error" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
                                            Height="18px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%" style="margin-top: -15px;">
                                <tr>
                                    <td>

                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                            AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GridView2_PageIndexChanging"
                                            OnRowDeleting="GridView2_RowDeleting" OnSelectedIndexChanging="GridView2_SelectedIndexChanging">
                                            <Columns>

                                                <asp:BoundField DataField="T12RNK" HeaderText="Rank Code" ReadOnly="True" SortExpression="T12RNK" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="T12RTE" HeaderText="TCL (%)" ReadOnly="True" SortExpression="T12RTE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T12STD" HeaderText="Start Date" ReadOnly="True" SortExpression="T12STD" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T12END" HeaderText="End Date" ReadOnly="True" SortExpression="T12END" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                    <ItemStyle Width="100px" HorizontalAlign="center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="T12BOT" HeaderText="No Limit BOT" ReadOnly="True" SortExpression="T12BOT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle Width="100px" HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="T12OUT" HeaderText="Up-Rank All Case" ReadOnly="True" SortExpression="T12OUT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="pk1s" runat="server" Width="250px" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="pk2s" runat="server" Width="250px" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="pk3s" runat="server" Width="250px" Visible="false"></asp:TextBox>

                                    </td>
                                </tr>
                            </table>

                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSqlAll" runat="server" Width="250px" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtSqlAll2" runat="server" Width="250px" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
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
                                                    <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label11" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddl_popup_SearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="RK" Selected="True">Ranking</asp:ListItem>
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
                                                    <asp:TextBox ID="txt_ProductType" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <dx:ASPxButton ID="btn_popup_search" runat="server" Text="Search" CssClass="AlignButtonInline" Width="80px" Height="24px"></dx:ASPxButton>
                                                    <dx:ASPxButton ID="Button5" runat="server" Text="Clear" CssClass="AlignButtonInline" Width="80px" Height="24px"></dx:ASPxButton>
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
                                        <br />
                                        <div style="width: 100%; height: 400px; overflow: scroll">
                                            <asp:GridView ID="gvRank" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                EmptyDataText="No records found" OnSelectedIndexChanging="gvRank_SelectedIndexChanging">
                                                <Columns>

                                                    <asp:BoundField DataField="ranking" HeaderText="Ranking" ReadOnly="True" SortExpression="ranking">
                                                        <ItemStyle Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="addition" HeaderText="Additional TCL(%)" ReadOnly="True"
                                                        SortExpression="T00TNM">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="strDate" HeaderText="Start Date" ReadOnly="True" SortExpression="strDate">
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="endDate" HeaderText="End Date" SortExpression="endDate" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#CCCC99" />
                                                <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                <RowStyle BackColor="#F7F7DE" />
                                            </asp:GridView>
                                        </div>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
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
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
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
                                                <td align="Center" width="50%">
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
                                                        OnClick="btnConfirm_Cancel_Click" Style="height: 25px">
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
                                PopupVerticalAlign="TopSides" HeaderText="Confirm Add" CloseAction="CloseButton"
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
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmOK" runat="server" Text="OK" Width="100px" OnClick="btnConfirmOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px"
                                                        OnClick="btnConfirm_Add_Cancel_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />

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

