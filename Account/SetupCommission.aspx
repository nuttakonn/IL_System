<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="SetupCommission.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Account.SetupCommission" %>
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
    <script language="javascript" type="text/javascript">


        var oldColor = '';



        function ChangeRowColor(rowID) {

            var color = document.getElementById(rowID).style.backgroundColor;

            if (color == 'yellow')

                document.getElementById(rowID).style.backgroundColor = oldColor;

            else document.getElementById(rowID).style.backgroundColor = 'yellow';

        }

    </script>
    <script type="text/javascript">
        function EnterEvent(e) {
        if (e.keyCode == 13) {
            __doPostBack('<%=btnAddRank.UniqueID%>', "");
        }
    }
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
                        Setup Vendor Commission
                    </HeaderTemplate>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                             <asp:HiddenField ID="ds_popup" runat="server"/>
                            <br />
                            <table width="100%" border="0">
                                <tr style="height: 30px;">
                                    <td style="width: 120px;">
                                        <asp:Label ID="Label17" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Vendor ID" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:TextBox runat="server" Height="20px" Width="100px" ID="vendorID" OnTextChanged="SELECT_VENDOR_ONCHANGE" autocomplete="off" AutoPostBack="true" MaxLength="13" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                        <%--<asp:TextBox runat="server" Height="20px" Width="100px" ID="vendorID"  autocomplete="off" MaxLength="13" OnKeyPress="return chkNumber(this)"></asp:TextBox>--%>
                                    </td>
                                    <td style="width: 20px;" align="center">

                                       
                                    </td>
                                    <td style="width: 220px;" colspan="2">
                                        <asp:TextBox runat="server" Height="20px" Width="220px" ID="vendorDescription"  OnTextChanged="SELECT_VENDOR_ONCHANGE_DES" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                    <td >
                                         <dx:ASPxButton ID="btnAddRank" runat="server" CssClass="button_bg_withe" OnClick="CLICK_SELECT_VENDOR" BackColor="White" EnableDefaultAppearance="False" Cursor="pointer">
                                            <Image Url="~\Images\icon\search.png" Width="20"></Image>
                                             <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                            <Border BorderStyle="None" />
                                            <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                        </dx:ASPxButton>
                                    </td>
                                    <td width="30"></td>
                                    <td style="width: 120px;" >
                                        <asp:Label ID="Label3" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Create Payment" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Height="20px" Width="100px" ID="createPayment"></asp:TextBox>

                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                                <tr style="height: 30px;">
                                    <td style="width: 140px;">
                                        <asp:Label ID="Label18" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Payment Vendor" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="width: 100px;" >
                                        <asp:TextBox ID="paymentVendor" runat="server" Width="100px" Height="20"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td style="width: 220px;" colspan="2">
                                        <asp:TextBox runat="server" Height="20px" Width="220px" ID="paymentDesciption"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td width="30"></td>
                                    <td style="width: 120px;">

                                        <asp:Label ID="Label4" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text=" Referance No." Width="120px"></asp:Label>
                                    </td>
                                    <td>:
            
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Height="20px" Width="100px" ID="referanceNo"></asp:TextBox>

                                    </td>
                                    <td colspan="2"></td>
                                </tr>
                                <tr style="height: 30px;">
                                    <td style="width: 120px;">
                                        <asp:Label ID="Label19" runat="server"
                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Commission Type" Width="120px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;">:
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="commissionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SELECT_CAMPAIGN_TYPE_ONCHANGE" Height="25" Width="108px">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td style="width: 30px;" colspan="1">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server"
                                                        Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text=" Due Day :" Width="80px"></asp:Label>
                                                </td>
                                                <td width="80">
                                                    <asp:DropDownList ID="dueDay" runat="server" Height="25" Width="80px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td style="width: 20px;" align="center" valign="middle">


                                        <dx:ASPxButton ID="btnEdit" runat="server" CssClass="button_bg_withe" OnClick="BTN_EDIT_COMISSION" BackColor="White" EnableDefaultAppearance="False" Cursor="pointer">
                                            <Image Url="~\Images\icon\edit.png" Width="20"></Image>

                                            <Border BorderStyle="None" />
                                            <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                        </dx:ASPxButton>

                                    </td>
                                   <td style="width: 20px;" align="center" valign="middle">
                                        <asp:ImageButton runat="server" AlternateText="ImageButton" ImageAlign="Left" ImageUrl="~\Images\icon\refresh.png" Height="18px" ID="btnRefresh" OnClick="CLICK_RE_COMMISSION"></asp:ImageButton>
</td>
                                    <td width="30">
                                         <asp:ImageButton runat="server" AlternateText="ImageButton" ImageAlign="Left" ImageUrl="~\Images\icon\save.png" Height="18px" ID="btnSave" OnClick="BTN_SAVE_COMISSION"></asp:ImageButton>
                                    </td>
                                    <td style="width: 20px;" align="left" valign="middle">
                                      
                                       
                                    </td>
                                    <td style="width: 20px;" align="center" valign="middle" >


                                    </td>
                                
                                    <td>
                                        <asp:TextBox runat="server" Width="60" Visible="false" ID="hisCommissionType"></asp:TextBox>
                                        <asp:TextBox runat="server" Width="60" Visible="false" ID="hisDueDay"></asp:TextBox>
                                        <asp:TextBox runat="server" Width="60" Visible="false" ID="setCommision"></asp:TextBox>


                                    </td>
                                    <td> </td>
                                </tr>
                            </table>
                            <br />

                            <dx:ASPxPageControl ID="ContentControlStep1" runat="server" ActiveTabIndex="0" EnableCallBacks="True" SaveStateToCookies="True" OnActiveTabChanged="ACTIVE_TAB_CHANGE" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                Width="100%" Theme="SoftOrange" ClientInstanceName="pageControl" EnableHierarchyRecreation="True" AutoPostBack="true">
                                <TabPages>

                                    <dx:TabPage Text="Step 1" Name="Step1" ActiveTabStyle-Font-Bold="true">
                                        <ActiveTabStyle Font-Bold="True"></ActiveTabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server">

                                                <table width="100%" border="1" style="border: 2px double #DDECFE;">
                                                    <tr>
                                                        <td width="20%" valign="top">
                                                            <asp:Panel ID="step1_box1" runat="server">
                                                                <table width="100%" class="bg1">
                                                                    <tr align="center" style="height: 20px;">
                                                                        <td>
                                                                            <asp:Label ID="Label36" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text="List Date of Vendor" Width="130px" Height="16px"></asp:Label></td>
                                                                    </tr>

                                                                </table>
                                                                <table width="100%">
                                                                    <tr align="center" style="height: 250px; background-color: #FFFFFF;">
                                                                        <td dir="ltr" headers="false" valign="top">
                                                                            <div style="height: 250px; overflow-y: scroll; width: auto;">
                                                                                <asp:GridView ID="gvListDateVendor" runat="server" AutoGenerateColumns="False" BackColor="White" OnRowDataBound="GV_ONBOUND_ROW_LISTVENDOR"
                                                                                    BorderColor="White" BorderStyle="None" CellPadding="4"
                                                                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                    ShowHeader="False" OnSelectedIndexChanging="GV_VENDOR_SELECTED_INDEX_CHANGE">
                                                                                    <Columns>

                                                                                        <asp:BoundField DataField="M01SDT" HeaderText="M01SDT" ReadOnly="True" SortExpression="M01SDT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField>

                                                                                            <ItemTemplate>
                                                                                                <div style="text-align: center; width: 10px;">
                                                                                                    -
                                                                                                </div>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="M01EDT" HeaderText="M01EDT" ReadOnly="True" SortExpression="M01EDT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                            <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                                        </asp:BoundField>

                                                                                        <asp:BoundField DataField="M01REF" HeaderText="M01REF" Visible="false" ReadOnly="True" SortExpression="M01REF" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                            <ItemStyle Width="100px" HorizontalAlign="right" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="M01BBM" Visible="false"></asp:BoundField>
                                                                                        <asp:BoundField DataField="M01BTY" Visible="false"></asp:BoundField>
                                                                                        <asp:BoundField DataField="M01RTY" Visible="false"></asp:BoundField>
                                                                                        <asp:BoundField DataField="M01I0P" Visible="false"></asp:BoundField>
                                                                                        <asp:BoundField DataField="M01CBO" Visible="false"></asp:BoundField>
                                                                                        <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                                        </asp:ButtonField>



                                                                                    </Columns>


                                                                                    <RowStyle BorderStyle="None" />


                                                                                </asp:GridView>
                                                                                 <asp:HiddenField ID="ds_gvListDateVendor" runat="server"/>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="highLightRow" Visible="false" runat="server" Height="20" Width="90px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td valign="top" width="50%">
                                                            <asp:Panel ID="step1_box2" runat="server">
                                                                <table width="100%" class="bg3" style="height: 45px;">
                                                                    <tr>
                                                                        <td width="50">
                                                                            <asp:Label ID="Label37" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text="Start date " Width="90px" Height="16px"></asp:Label>





                                                                        </td>
                                                                        <td width="95" valign="middle">


                                                                            <asp:TextBox ID="startDate" runat="server" Height="20" Width="90px"></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                                Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate">
                                                                            </cc1:CalendarExtender>
                                                                        </td>
                                                                        <td width="30">
                                                                            <%--<dx:ASPxButton ID="calendarStart" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                                <Border BorderStyle="None" />
                                                                                <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                            </dx:ASPxButton>--%>
                                                                            <asp:ImageButton ID="calendarStart" runat="server" ImageAlign="Bottom" 
                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" />

                                                                        </td>
                                                                        <td width="30" align="center">
                                                                            <asp:Label ID="Label38" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text=" - " Width="20px" Height="16px"></asp:Label>
                                                                        </td>
                                                                        <td width="95">
                                                                            <asp:TextBox ID="endDate" runat="server" Height="20" Width="90px"></asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                                                Format="dd/MM/eeee" PopupButtonID="calendarEnd" TargetControlID="endDate">
                                                                            </cc1:CalendarExtender>

                                                                        </td>
                                                                        <td width="30">
                                                                            <%--<dx:ASPxButton ID="calendarEnd" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                                <Border BorderStyle="None" />
                                                                                <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                            </dx:ASPxButton>--%>
                                                                            <asp:ImageButton ID="calendarEnd" runat="server" ImageAlign="Bottom" 
                        ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>

                                                                </table>
                                                                <br />
                                                                <table>

                                                                    <tr>
                                                                        <td>Budget Branch
                                                                        </td>
                                                                        <td>:
                                                                        </td>
                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                            <asp:DropDownList ID="listboxBBM" runat="server" Height="25" Width="200px">
                                                                            </asp:DropDownList>

                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>Budget Type
                                                                        </td>
                                                                        <td>:
                                                                        </td>
                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                            <asp:DropDownList ID="listboxBTY" runat="server" Height="25" Width="200px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Rate Type
                                                                        </td>
                                                                        <td>:
                                                                        </td>
                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">

                                                                            <asp:DropDownList ID="listboxRTY" runat="server" Height="25" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_LISTBOX">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Include Interest rate
                                                                        </td>
                                                                        <td>:
                                                                        </td>
                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                            <asp:DropDownList ID="listboxIIR" runat="server" Height="25" Width="200px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Commission Base on
                                                                        </td>
                                                                        <td>:
                                                                        </td>
                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                            <asp:DropDownList ID="listboxCBO" runat="server" Height="25" Width="200px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="40%" valign="top">
                                                            <asp:Panel ID="step1_box3" runat="server">
                                                                <table width="100%" class="bg4">
                                                                    <tr align="center" style="height: 20px;">
                                                                        <td>
                                                                            <asp:Label ID="Label39" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text="Commission Rate" Width="130px" Height="16px"></asp:Label></td>
                                                                    </tr>

                                                                </table>

                                                                <table width="100%">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="textId" runat="server" Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Amount
                                                                        </td>
                                                                        <td>:
                                                                         <asp:TextBox runat="server" Style="text-align: right" MaxLength="3" Height="20px" Width="100px" Text="0.00" ID="txtStartAmount" Enabled="false" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                            -
                                                                         <asp:TextBox runat="server" Style="text-align: right" Height="20px" Width="100px" Text="0.00" ID="txtEndAmount" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Rate
                                                                        </td>
                                                                        <td>:
                                                                       <asp:TextBox runat="server" Style="text-align: right" MaxLength="5" Height="20px" Width="100px" AutoComplete="off" placeholder="00.00" ID="txtRate" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                            %
                                                   &nbsp;&emsp;<asp:Button Text="+" runat="server" CssClass="cursorPointer" Height="22" ID="btnAddRows" OnClick="CLICK_ADD_ROW" />
                                                                            &nbsp; 
                                                                        <asp:Button Text="-" CssClass="cursorPointer" ID="btnDeleteRows" runat="server" OnClick="CLICK_DELETE_ROW" Height="22" Width="24" />

                                                                        </td>
                                                                    </tr>

                                                                </table>

                                                                <br />
                                                                <table width="100%" class="auto-style1" border="1" style="border-collapse: collapse;">
                                                                    <tr align="center" class="bg4" style="height: 20px;">
                                                                        <td>
                                                                            <asp:Label ID="Label40" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text="START AMOUNT" Width="130px" Height="16px"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="Label41" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text="END AMOUNT" Width="130px" Height="16px"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="Label42" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                Text="RATE (%)" Width="130px" Height="16px"></asp:Label></td>
                                                                    </tr>


                                                                    <tr align="center" style="height: 150px; background-color: #FFFFFF;">
                                                                        <td colspan="3" valign="top">
                                                                            <div style="height: 150px; overflow-y: scroll;">
                                                                                <asp:GridView ID="gvAddRows" runat="server" CssClass="Grid" AutoGenerateColumns="False" Width="100%"
                                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                                                    <AlternatingRowStyle BackColor="White" />
                                                                                    <Columns>

                                                                                        <asp:BoundField DataField="Id" Visible="false" HeaderText="Id" ItemStyle-Width="120">
                                                                                            <ItemStyle Width="120px"></ItemStyle>

                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="M02SAM" HeaderText="M02SAM" ItemStyle-Width="120">
                                                                                            <ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="M02EAM" HeaderText="M02EAM" ItemStyle-Width="120">
                                                                                            <ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="M02CMR" HeaderText="M02CMR" ItemStyle-Width="120">
                                                                                            <ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
                                                                                        </asp:BoundField>
                                                                                    </Columns>
                                                                                    <%--<EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />--%>
                                                                                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                                                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                                                                    <RowStyle BackColor="#FFFFB9" ForeColor="#333333" />
                                                                                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                                                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                                    <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>

                                    <dx:TabPage Text="Step 2" Name="Step2" ActiveTabStyle-Font-Bold="true">
                                        <ActiveTabStyle Font-Bold="True"></ActiveTabStyle>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ASPxPageControl1" runat="server">
                                                <table>

                                                    <tr>
                                                        <td>
                                                            <table width="100%" class="bg1" style="margin-top: 6px;">
                                                                <tr align="center">
                                                                    <td>
                                                                        <asp:Label ID="Label43" runat="server"
                                                                            Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                            Text="Seq" Width="120px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table width="100%">
                                                                <tr style="background-color: #FFFFFF; height: 484px;">
                                                                    <td valign="top">
                                                                        <asp:GridView ID="gvSeqNo" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                            BorderColor="White" BorderStyle="None" CellPadding="4"
                                                                            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                                            ShowHeader="False" OnSelectedIndexChanging="GV_SEQNO_SELECTED_INDEX_CAHNGE">
                                                                            <Columns>

                                                                                <asp:BoundField DataField="M03SEQ" HeaderText="M03SEQ" ReadOnly="True" SortExpression="M01SDT" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                    <ItemStyle Width="100px" HorizontalAlign="center" />
                                                                                </asp:BoundField>


                                                                                <asp:BoundField DataField="M01BBM" Visible="false"></asp:BoundField>
                                                                                <asp:BoundField DataField="M01BTY" Visible="false"></asp:BoundField>
                                                                                <asp:BoundField DataField="M01RTY" Visible="false"></asp:BoundField>
                                                                                <asp:BoundField DataField="M01I0P" Visible="false"></asp:BoundField>
                                                                                <asp:BoundField DataField="M01CBO" Visible="false"></asp:BoundField>
                                                                                <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                                </asp:ButtonField>

                                                                            </Columns>


                                                                            <RowStyle BorderStyle="None" />


                                                                            <SelectedRowStyle BackColor="#FFFFB9" BorderStyle="None" BorderColor="#FFFFB9" ForeColor="Black" />
                                                                        </asp:GridView>
                                                                         <asp:HiddenField ID="ds_gvSeqNo" runat="server"/>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" width="100%">
                                                            <dx:ASPxPageControl ID="ContentControlStep2" runat="server" ActiveTabIndex="1" EnableCallBacks="True" SaveStateToCookies="True" OnActiveTabChanged="ACTIVE_TAB2_CHANGE" AutoPostBack="true" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                                CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                                                                Width="100%" Theme="SoftOrange" EnableHierarchyRecreation="True">
                                                                <TabPages>

                                                                    <dx:TabPage Text="Budget Contract" Name="BudgetContract" ActiveTabStyle-Font-Bold="true">
                                                                        <ActiveTabStyle Font-Bold="True"></ActiveTabStyle>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl2" runat="server">

                                                                                <table width="65%" border="1" style="border: 2px double #DDECFE;">
                                                                                    <tr>
                                                                                        <td valign="top" width="100%">
                                                                                            <asp:Panel ID="step2_box1" runat="server">
                                                                                                <table width="100%" class="bg3" style="height: 45px;">
                                                                                                    <tr>
                                                                                                        <td width="60px">
                                                                                                            <asp:Label ID="Label1" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="Seq No." Width="60px" Height="16px"></asp:Label>

                                                                                                        </td>
                                                                                                        <td width="80px">
                                                                                                            <asp:TextBox runat="server" Height="20px" Width="50px" ID="seqNo" Style="text-align: center;"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td width="100px" style="text-align: right;">
                                                                                                            <asp:Label ID="Label44" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="Start date " Width="80px" Height="16px"></asp:Label>

                                                                                                        </td>

                                                                                                        <td width="95" valign="middle">

                                                                                                            <asp:TextBox ID="startDateStep2" runat="server" Height="20" Width="90px"></asp:TextBox>
                                                                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
                                                                                                                Format="dd/MM/eeee" PopupButtonID="calendarStartStep2" TargetControlID="startDateStep2">
                                                                                                            </cc1:CalendarExtender>
                                                                                                        </td>
                                                                                                        <td width="30">
                                                                                                            <dx:ASPxButton ID="calendarStartStep2" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                            </dx:ASPxButton>
                                                                                                        </td>
                                                                                                        <td width="30px" align="center">
                                                                                                            <asp:Label ID="Label45" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text=" - " Width="20px" Height="16px"></asp:Label>
                                                                                                        </td>

                                                                                                        <td width="95">
                                                                                                            <asp:TextBox ID="endDateStep2" runat="server" Height="20" Width="90px"></asp:TextBox>
                                                                                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True"
                                                                                                                Format="dd/MM/eeee" PopupButtonID="calendarEndStep2" TargetControlID="endDateStep2">
                                                                                                            </cc1:CalendarExtender>

                                                                                                        </td>
                                                                                                        <td width="30">
                                                                                                            <dx:ASPxButton ID="calendarEndStep2" runat="server" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>

                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                                                            </dx:ASPxButton>
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                </table>
                                                                                                <br />
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>Budget Branch
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">

                                                                                                            <asp:DropDownList ID="budgetBranch" runat="server" Height="25" Width="200px" Style="background-color: #CAFFE3; border: 1px solid #BDDBD4;">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Type
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:DropDownList Style="background-color: #CAFFE3; border: 1px solid #BDDBD4;" ID="budgetType" runat="server" Height="26" Width="200px">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Contract
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:DropDownList ID="budgetContract" runat="server" Height="25" Width="200px">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Amount
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:TextBox runat="server" Text="0.00" CssClass="rightAlign" Height="20px" Width="192px" ID="txtAmount" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Criteria Type
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:DropDownList ID="budgetCriteriaType" runat="server" Height="25" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_BUDGET_CRITERIA">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Amount
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:TextBox runat="server" Text="0.00" CssClass="rightAlign" Height="20px" Width="192px" ID="budgetAmount" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td>Bath</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Percentage
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:TextBox runat="server" Text="0.00" CssClass="rightAlign" Height="20px" Width="192px" ID="budgetPercentage" MaxLength="5" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td>%</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Type Comparative
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:DropDownList ID="budgetTypeComparative" runat="server" Height="25" Width="200px">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget App Type
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:DropDownList ID="budgetAppType" runat="server" AutoPostBack="true" Height="25" Width="200px" OnSelectedIndexChanged="ONCHANGE_BUDGET_APP">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Budget Product Type
                                                                                                        </td>
                                                                                                        <td>:
                                                                                                        </td>
                                                                                                        <td style="padding-bottom: 5px; padding-top: 5px;">
                                                                                                            <asp:DropDownList ID="budgetProductType" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_BUDGET_PRODUCT" runat="server" Height="25" Width="200px">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td></td>
                                                                                                    </tr>

                                                                                                </table>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                        <td valign="top">
                                                                                            <asp:Panel ID="step2_box2" runat="server">
                                                                                                <table width="100%" class="bg4">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label46" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="Budget / Month" Width="130px" Height="16px"></asp:Label></td>
                                                                                                    </tr>

                                                                                                </table>

                                                                                                <table width="280" class="bg1" border="1" style="border-collapse: collapse;">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td width="30">
                                                                                                            <asp:Label ID="Label47" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="MONTH" Width="130px" Height="16px"></asp:Label></td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label48" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="AMOUNT" Width="130px" Height="16px"></asp:Label></td>

                                                                                                    </tr>

                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">1</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month1" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>

                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">2</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month2" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">3</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month3" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">4</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month4" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">5</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month5" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">6</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month6" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">7</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month7" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">8</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month8" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">9</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month9" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">10</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month10" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">11</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month11" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr style="background-color: #FFFFFF;">
                                                                                                        <td align="center">12</td>
                                                                                                        <td align="right">
                                                                                                            <asp:TextBox runat="server" ID="month12" Text="0.00" Style="text-align: right; border-color: #FFFFFF;" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox></td>
                                                                                                    </tr>



                                                                                                </table>

                                                                                            </asp:Panel>
                                                                                        </td>

                                                                                    </tr>
                                                                                </table>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>

                                                                    <dx:TabPage Text="Application Type & Product Type" Name="ApplicationAndProduct" ActiveTabStyle-Font-Bold="true">
                                                                        <ActiveTabStyle Font-Bold="True"></ActiveTabStyle>
                                                                        <ContentCollection>
                                                                            <dx:ContentControl ID="ContentControl3" runat="server">

                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td rowspan="2" width="300" valign="top">
                                                                                            <asp:Panel runat="server" ID="step2_applicationType">
                                                                                                <table width="100%" class="bg4">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label49" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="Application Type" Width="130px" Height="16px"></asp:Label></td>
                                                                                                    </tr>

                                                                                                </table>
                                                                                                <table width="100%" style="border: 1px solid #83B2D1; border-collapse: collapse;">

                                                                                                    <tr class="bg3">
                                                                                                        <td width="190">
                                                                                                            <asp:TextBox runat="server" Height="20px" Width="180px" ID="readApplicationType" ReadOnly="true" Style="margin: 5px;"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td width="30">
                                                                                                            <dx:ASPxButton ID="applicationTypes" runat="server" CssClass="button_bg_sky" OnClick="CLICK_SELECT_APPLICATION_TYPE" BackColor="#EFF4F9" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~\Images\icon\search.png" Width="20"></Image>

                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                                                            </dx:ASPxButton>
                                                                                                        </td>
                                                                                                        <td>&nbsp; 
                                                                
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <table width="100%" class="bg1" border="1" style="border-collapse: collapse;">
                                                                                                    <tr style="height: 20px;">
                                                                                                        <td width="100" align="center">
                                                                                                            <asp:Label ID="Label50" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="TYPE" Width="90px" Height="16px"></asp:Label></td>
                                                                                                        <td align="center">
                                                                                                            <asp:Label ID="Label51" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="DESCRIPTION" Width="130px" Height="16px"></asp:Label></td>

                                                                                                    </tr>



                                                                                                </table>
                                                                                                <div style="height: 345px; overflow-y: auto; background-color: #F8F8F8;">
                                                                                                    <asp:GridView ID="gvAddRowsApplication" runat="server" CssClass="Grid" EmptyDataText="No records found" AutoGenerateColumns="False" Width="100%" OnRowDeleting="CLICK_DELETE_ROW_APPLICATION_TYPE"
                                                                                                        EnableModelValidation="True" ForeColor="#333333" ShowHeader="False">
                                                                                                        <AlternatingRowStyle BackColor="White" />

                                                                                                        <Columns>

                                                                                                            <asp:BoundField DataField="IdApp" Visible="false" HeaderText="IdApp" ItemStyle-Width="120">
                                                                                                                <ItemStyle></ItemStyle>

                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="GN61CD" HeaderText="GN61CD" ItemStyle-Width="100">
                                                                                                                <ItemStyle Width="100px" HorizontalAlign="Center"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="GN61DT" HeaderText="GN61DT">
                                                                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~/Images/icon/click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                                                            </asp:CommandField>
                                                                                                        </Columns>
                                                                                                        <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                                                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                                                                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                                                                                        <RowStyle BackColor="#FFFFB9" ForeColor="#333333" />
                                                                                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                                                                        <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                                                        <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                                                        <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                                                        <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </asp:Panel>
                                                                                        </td>

                                                                                        <td valign="top">
                                                                                            <asp:Panel runat="server" ID="step2_productType">
                                                                                                <table width="100%" class="bg4">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label52" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="Product Type" Width="130px" Height="16px"></asp:Label></td>
                                                                                                    </tr>

                                                                                                </table>
                                                                                                <table width="100%" style="border: 1px solid #83B2D1; border-collapse: collapse;">

                                                                                                    <tr class="bg3">
                                                                                                        <td width="100">
                                                                                                            <asp:TextBox runat="server" Height="20px" Width="230px" ReadOnly="true" ID="readProductType" Style="margin: 5px;"></asp:TextBox>
                                                                                                            <asp:TextBox runat="server" Height="20px" Width="230px" ReadOnly="true" ID="readProductTypeHidden" Visible="false"></asp:TextBox>

                                                                                                        </td>
                                                                                                        <td width="40px;">
                                                                                                            <dx:ASPxButton ID="productTypesSearch" runat="server" CssClass="button_bg_sky" OnClick="CLICK_SELECT_PRODUCT_TYPE" BackColor="#EFF4F9" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~\Images\icon\search.png" Width="20"></Image>

                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                                                            </dx:ASPxButton>
                                                                                                        </td>
                                                                                                         <asp:HiddenField ID="ds_gvProductType" runat="server"/>
                                                                                                        <td width="185">
                                                                                                            <asp:DropDownList ID="ddlProductType" runat="server" Height="25" Width="180px" Style="margin: 5px;">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                        <td>

                                                                                                            <asp:Button Text="+" runat="server" CssClass="cursorPointer" Height="22" ID="addProductType" Enabled="false" OnClick="CLICK_ADD_PRODUCT_TYPE" />&nbsp;
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>

                                                                                                <table width="100%" class="bg1" border="1" style="border-collapse: collapse;">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td width="80">
                                                                                                            <asp:Label ID="Label53" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="TYPE" Width="70px" Height="16px"></asp:Label></td>
                                                                                                        <td width="350">
                                                                                                            <asp:Label ID="Label54" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="DESCRIPTION" Width="340px" Height="16px"></asp:Label></td>
                                                                                                        <td width="80">
                                                                                                            <asp:Label ID="Label55" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="CODE" Width="70px" Height="16px"></asp:Label></td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label56" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="DESCRIPTION" Width="130px" Height="16px"></asp:Label></td>
                                                                                                        <td width="79" align="left">
                                                                                                            <asp:Label ID="Label23" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="SELECT" Width="70px" Height="16px"></asp:Label></td>

                                                                                                    </tr>


                                                                                                </table>
                                                                                                <div style="height: 128px; overflow-y: scroll; background-color: #F8F8F8;">
                                                                                                    <asp:GridView ID="gvAddRowsProductType" runat="server" CssClass="Grid" EmptyDataText="No records found" AutoGenerateColumns="False" Width="100%"
                                                                                                        OnSelectedIndexChanging="DATA_PRODUCT_SELECTED_INDEX_CAHNGE" EnableModelValidation="True" ForeColor="#333333" ShowHeader="False" OnRowDeleting="CLICK_DELETE_ROW_PRODUCT_TYPE">
                                                                                                        <AlternatingRowStyle BackColor="White" />

                                                                                                        <Columns>

                                                                                                            <asp:BoundField DataField="IdProduct" Visible="false" HeaderText="IdProduct" ItemStyle-Width="120">
                                                                                                                <ItemStyle></ItemStyle>

                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="T40TYP" ItemStyle-Width="100">
                                                                                                                <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="T40DES">
                                                                                                                <ItemStyle Width="350px" HorizontalAlign="Left"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="T71ITM" ItemStyle-Width="100">
                                                                                                                <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="T71DES">
                                                                                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                                                                <ItemStyle Width="43px" HorizontalAlign="Center" />
                                                                                                            </asp:ButtonField>
                                                                                                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                                                            </asp:CommandField>
                                                                                                        </Columns>
                                                                                                        <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                                        <FooterStyle BackColor="#990000" ForeColor="White" />
                                                                                                        <HeaderStyle BackColor="#990000" ForeColor="White" />
                                                                                                        <PagerStyle BackColor="#CAFFE3" ForeColor="#333333" HorizontalAlign="Center" />
                                                                                                        <RowStyle BackColor="#FFFFB9" ForeColor="#333333" />
                                                                                                        <SelectedRowStyle BackColor="#CAFFE3" />


                                                                                                        <SortedDescendingCellStyle BackColor="#FCF6C0" />

                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Panel runat="server" ID="step2_productCode">
                                                                                                <table width="100%" class="bg4">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td>
                                                                                                            <asp:Label ID="Label57" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="Product Code" Width="130px" Height="16px"></asp:Label></td>
                                                                                                    </tr>

                                                                                                </table>
                                                                                                <table width="100%" style="border: 1px solid #83B2D1; border-collapse: collapse;">

                                                                                                    <tr class="bg3">
                                                                                                        <td width="75">
                                                                                                            <asp:TextBox runat="server" Height="20px" Width="50px" ReadOnly="true" ID="productTypeSelect" Style="text-align: center; margin: 5px;"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td width="230">
                                                                                                            <asp:TextBox runat="server" Height="20px" Width="230px" ReadOnly="true" ID="readProductCode" Style="margin: 5px;"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td width="30" align="center" valign="middle">
                                                                                                            <dx:ASPxButton ID="productCodes" runat="server" CssClass="button_bg_sky" Enabled="false" Style="padding-top: -2px;" OnClick="CLICK_SELECT_PRODUCT_CODE" BackColor="#EFF4F9" EnableDefaultAppearance="False" Cursor="pointer">
                                                                                                                <Image Url="~\Images\icon\search.png" Width="20"></Image>

                                                                                                                <Border BorderStyle="None" />
                                                                                                                <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                                                            </dx:ASPxButton>
                                                                                                             <asp:HiddenField ID="ds_gvProductCode" runat="server"/>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox runat="server" ID="productTypeSend" Visible="false"></asp:TextBox>
                                                                                                            <asp:TextBox runat="server" ID="productDesSend" Visible="false"></asp:TextBox>
                                                                                                        </td>

                                                                                                    </tr>
                                                                                                </table>

                                                                                                <table width="100%" class="bg1" border="1" style="border-collapse: collapse;">
                                                                                                    <tr align="center" style="height: 20px;">
                                                                                                        <td width="73">
                                                                                                            <asp:Label ID="Label58" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="TYPE" Width="70px" Height="16px"></asp:Label></td>
                                                                                                        <td width="72">
                                                                                                            <asp:Label ID="Label59" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="CODE" Width="70px" Height="16px"></asp:Label></td>
                                                                                                        <td width="350">
                                                                                                            <asp:Label ID="Label60" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                                                                                Text="DESCRIPTION" Height="16px"></asp:Label></td>


                                                                                                    </tr>


                                                                                                </table>
                                                                                                <div style="height: 128px; overflow-y: scroll; background-color: #F8F8F8;">
                                                                                                    <asp:GridView ID="gvAddRowsProductCode" runat="server" CssClass="Grid" EmptyDataText="No records found" AutoGenerateColumns="False" Width="100%" OnSelectedIndexChanging="CODE_SELECTED_INDEX_CAHNGE" OnRowDeleting="CLICK_DELETE_ROW_PRODUCT_CODE"
                                                                                                        EnableModelValidation="True" ForeColor="#333333" ShowHeader="False">
                                                                                                        <AlternatingRowStyle BackColor="White" />

                                                                                                        <Columns>

                                                                                                            <asp:BoundField DataField="T41TYP" ItemStyle-Width="80">
                                                                                                                <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>

                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="T41COD" ItemStyle-Width="80">
                                                                                                                <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="T41DES">
                                                                                                                <ItemStyle HorizontalAlign="Left" Width="350px"></ItemStyle>
                                                                                                            </asp:BoundField>

                                                                                                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                                                            </asp:CommandField>
                                                                                                        </Columns>
                                                                                                        <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                                                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                                                                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                                                                                        <RowStyle BackColor="#FFFFB9" ForeColor="#333333" />
                                                                                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                                                                        <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                                                        <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                                                        <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                                                        <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </asp:Panel>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </dx:ContentControl>
                                                                        </ContentCollection>
                                                                    </dx:TabPage>

                                                                </TabPages>
                                                            </dx:ASPxPageControl>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>

                                </TabPages>
                            </dx:ASPxPageControl>
                            <%--End tab containMain--%>

                            <br />
                            <table style="align-content: center" width="100%">
                                <tr align="center">
                                    <td width="30%"></td>
                                    <td width="200">
                                        <dx:ASPxButton ID="btnMainInsert" OnClick="CLICK_MAIN_INSERT" runat="server" Text="INSERT" Height="25" CssClass="imgNormal" Cursor="pointer">
                                            <Image Url="~\Images\icon\insert.png" Width="16"></Image>
                                            <DisabledStyle CssClass="imgGreyscal">
                                            </DisabledStyle>
                                        </dx:ASPxButton>
                                    </td>

                                    <td width="200">
                                        <dx:ASPxButton ID="btnMainEdit" runat="server" value="modeEdit" Text="EDIT" CssClass="imgNormal" OnClick="CLICK_MAIN_EDIT" Cursor="pointer">
                                            <Image Url="~\Images\icon\edit.png" Width="16"></Image>
                                            <DisabledStyle CssClass="imgGreyscal">
                                            </DisabledStyle>
                                        </dx:ASPxButton>
                                    </td>
                                    <td width="200">
                                        <dx:ASPxButton ID="btnMainDelete" runat="server" value="modeDelete" Text="DELETE" CssClass="imgNormal" Cursor="pointer" OnClick="CLICK_MAIN_DELETE">
                                            <Image Url="~\Images\icon\click_trash.png" Width="16"></Image>

                                            <DisabledStyle CssClass="imgGreyscal">
                                            </DisabledStyle>
                                        </dx:ASPxButton>
                                    </td>
                                    <td width="200">
                                        <dx:ASPxButton ID="btnMainCancel" runat="server" value="modeCancel" Text="CANCEL" CssClass="imgNormal" OnClick="CLICK_CLEAR_ALL" Cursor="pointer">
                                            <Image Url="~\Images\icon\close.png" Width="16"></Image>
                                            <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                        </dx:ASPxButton>
                                    </td>

                                    <td width="200">
                                        <dx:ASPxButton ID="btnMainSave" runat="server" value="modeSave" Text="SAVE" CssClass="imgNormal" Cursor="pointer" OnClick="CLICK_MAIN_SAVE">
                                            <Image Url="~\Images\icon\save.png" Width="16"></Image>
                                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                            <DisabledStyle CssClass="imgGreyscal">
                                            </DisabledStyle>
                                        </dx:ASPxButton>
                                    </td>
                                    <td width="30%">
                                        <asp:TextBox ID="checkModeStatus" runat="server" Visible="false"> </asp:TextBox>
                                        <asp:TextBox ID="checkStepStatus" runat="server" Visible="false"> </asp:TextBox>
                                    </td>

                                </tr>

                            </table>
                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server"
                                ClientInstanceName="LoadingPanel" Height="170px" Width="320px">
                            </dx1:ASPxLoadingPanel>
                            <dx:ASPxPopupControl ID="PopupAlertRate" ClientInstanceName="PopupAlertRate"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="Middle" HeaderText="คำแนะนำ"
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
                                                    <dx:ASPxButton ID="popupAlerRate" runat="server" Text="OK" Width="100px" OnClick="CLICK_CONFIRM_ALERT">

                                                        <ClientSideEvents Click="function(s, e) { PopupAlertRate.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAlertApp" ClientInstanceName="PopupAlertApp"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="Middle" HeaderText="คำแนะนำ"
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
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="yesApp" runat="server" value="Yes" Text="Yes" Width="100px"
                                                        OnClick="BTN_CANCEL_ITEM_APP">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertApp.Hide();}" />

                                                    </dx:ASPxButton>

                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="noApp" runat="server" value="No" Text="No" Width="100px" OnClick="BTN_OK_CHECK_ITEM_APP">

                                                        <ClientSideEvents Click="function(s, e) { PopupAlertApp.Hide();LoadingPanel.Show(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>

                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAlertProduct" ClientInstanceName="PopupAlertProduct"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="Middle" HeaderText="คำแนะนำ"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <dx:ASPxLabel ID="lblMsgAlertProduct" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="yesProduct" runat="server" value="Yes" Text="Yes" Width="100px"
                                                        OnClick="BTN_CANCEL_ITEM_PRODUCT">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertProduct.Hide();}" />

                                                    </dx:ASPxButton>

                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="noProduct" runat="server" value="No" Text="No" Width="100px" OnClick="BTN_OK_CHECK_ITEM_PRODUCT">

                                                        <ClientSideEvents Click="function(s, e) { PopupAlertProduct.Hide();LoadingPanel.Show(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>

                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupAlertClearStep2" ClientInstanceName="PopupAlertClearStep2"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="Middle" HeaderText="คำแนะนำ"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <dx:ASPxLabel ID="lblClearStep2" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <%--<table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="100%">
                                                     <dx:ASPxButton ID="btnOkClearStep2" runat="server" value="No" Text="OK" Width="100px">

                                                        <ClientSideEvents Click="function(s, e) { PopupAlertClearStep2.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                   
                                                </td>

                                            </tr>

                                        </table>--%>
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnYesConfirm" runat="server" Text="Yes" Width="100px" OnClick="CLICK_YES_CONFIRM_BACKSTEP">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertClearStep2.Hide();LoadingPanel.Show(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="bttNoconfirm" runat="server" Text="No" Width="100px" OnClick="CLICK_NO_CONFIRM_BACKSTEP">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertClearStep2.Hide();}" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupSearchAlert" ClientInstanceName="PopupSearchAlert"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="Middle" HeaderText="คำแนะนำ"
                                AllowDragging="True" Modal="True" Width="300px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" CloseAction="CloseButton">
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
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/alert.png" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>หากไม่ระบุตัวเลขของร้านค้าเลย อาจทำให้การค้นหาใช้เวลานาน เพื่อเป็นการหลีกเลี่ยงกรุณากรอกข้อมูลตัวเลขตั้งแต่ 3 ตัวขึ้นไป หรือระบุชื่อ vendor
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="popUpOK" runat="server" Text="OK" Width="100px" OnClick="BTN_CONFIRM_SEARCH_CLICK">
                                                        <ClientSideEvents Click="function(s, e) { PopupSearchAlert.Hide(); }" />
                                                        <%--<ClientSideEvents Click="function(s, e) { PopupSearchAlert.Hide(); }"></ClientSideEvents>--%>
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="popUpCancel" runat="server" value="Cancel" Text="Cancel" Width="100px"
                                                        OnClick="BTN_CONFIRM_SEARCH_CANCEL_CLICK">
                                                        <ClientSideEvents Click="function(s, e) { PopupSearchAlert.Hide();LoadingPanel.Show();}" />

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
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }"></ClientSideEvents>
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
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/success.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsgSuccess" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #1F6E42;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="btnSuccess" runat="server" Text="OK" Width="100px" OnClick="SET_CLEAR_ADD">

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
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="320px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
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
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/icon/deletenew.png" /><br />
                                                    <br />
                                                    <b style="color: darkred;">Do you really want to delete the data ?</b>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="confirmMainDelete" runat="server" Text="OK" Width="100px" OnClick="CONFIRM_MAIN_DELETE">

                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide(); LoadingPanel.Show(); }"></ClientSideEvents>

                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="cancelMainDelete" runat="server" Text="Cancel" Width="100px" OnClick="CANCEL_MAIN_DELETE">

                                                        <ClientSideEvents Click="function(s, e) { PopupConfirmDelete.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="Popup_SelectVendor" ClientInstanceName="Popup_SelectVendor"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="TopSides" HeaderText="Vendor Code" CloseAction="CloseButton"
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
                                                    <asp:DropDownList ID="ddl_popup_SearchBy" runat="server" Width="150px" Height="24">
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
                                                    <asp:TextBox ID="txt_Detail" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">

                                                    <asp:Button ID="btn_popup_search" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_VENDOR" CssClass="cursorPointer" />&nbsp;&nbsp;
                                                    <asp:Button ID="btn_popup_clear" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="CLEAR_POPUP_VENDOR" CssClass="cursorPointer" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="psVendor" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="PS_VENDOR_SELECTED_PAGEINDEX_CHANGE"
                                                OnSelectedIndexChanging="PS_VENDOR_SELECTED_INDEX_CHANGE">
                                                <Columns>

                                                    <asp:BoundField DataField="P10VEN" HeaderText="Code" ReadOnly="True" SortExpression="P10VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                         <HeaderStyle HorizontalAlign="Center" Width="120"></HeaderStyle>

                                                                    <ItemStyle Width="120px"></ItemStyle>
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="P10NAM" HeaderText="Vendor Name" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                    </asp:BoundField>
                                                 <%--   <asp:BoundField></asp:BoundField>--%>
                                                    <asp:BoundField DataField="T71DES" Visible="false"></asp:BoundField>
                                                    <asp:BoundField DataField="P10CPD" Visible="false"></asp:BoundField>
                                                    <asp:BoundField DataField="P10PTY" Visible="false"></asp:BoundField>


                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                       <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>

                                                                    <ItemStyle Width="70px"></ItemStyle>
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
                <dx:ASPxPopupControl ID="Popup_ApplicationType" ClientInstanceName="Popup_ApplicationType"
                    ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="TopSides" HeaderText="Application Type" CloseAction="CloseButton"
                    AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="300px"
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
                                <asp:HiddenField ID="ds_gvApplicationType" runat="server"/>
                                <tr>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                        <asp:Label ID="Label2" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Search By" Width="70px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label6" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                            Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                        <asp:DropDownList ID="ddlSearchApptype" runat="server" Width="150px" Height="24">
                                            <asp:ListItem Value="CAT" Selected="True">Code</asp:ListItem>

                                            <%-- <asp:ListItem Value="DAT">Description</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                        <asp:Label ID="Label7" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                            Text="Text Search" Width="100px"></asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                        <asp:Label ID="Label8" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                            Width="5px" Height="16px">:</asp:Label>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                        <asp:TextBox ID="txtSearchAppType" runat="server" Width="160px" Height="18"></asp:TextBox>
                                    </td>
                                    <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                        colspan="2">
                                        <asp:Button ID="appTypeSearch" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_APP_TYPE"
                                            CssClass="cursorPointer" />&nbsp;&nbsp;
                                                    <asp:Button ID="appTypeClear" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="CLEAR_POPUP_APPTYPE" CssClass="cursorPointer" />
                                    </td>
                                </tr>
                            </table>

                            <br />
                            <div style="width: 100%; height: 200px; overflow: scroll">
                                <asp:GridView ID="gv_applicationType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="APPLICATION_TYPE_SELECTED_PAGEINDEX_CAHNGE"
                                    OnSelectedIndexChanging="APPLICATION_TYPE_SELECTED_INDEX_CAHNGE">

                                    <Columns>

                                        <asp:BoundField DataField="GN61CD" HeaderText="Code" ReadOnly="True" SortExpression="P10VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="GN61DT" HeaderText="Description" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
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
                                        <asp:Button ID="productTypeSearch" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_TYPE" />&nbsp;&nbsp;
                                                    <asp:Button ID="productTypeClear" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="CLEAR_POPUP_PRODUCTTYPE" CssClass="cursorPointer" />
                                    </td>
                                </tr>
                            </table>

                            <br />
                            <div style="width: 100%; height: 450px; overflow: scroll">


                                <asp:GridView ID="gv_ProductType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="PRODUCT_TYPE_PAGEINDEX_CAHNGE"
                                    OnSelectedIndexChanging="PRODUCT_SELECTED_INDEX_CAHNGE">
                                    <Columns>

                                        <asp:BoundField DataField="T40TYP" HeaderText="Code" ReadOnly="True" SortExpression="P10VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="T40DES" HeaderText="Description" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
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
                                            CssClass="cursorPointer" />&nbsp;&nbsp;
                                                    <asp:Button ID="productCodeClear" runat="server" Text="Clear" Width="80px" Height="24px" CssClass="cursorPointer" OnClick="CLEAR_POPUP_PRODUCTCODE" />
                                    </td>
                                </tr>
                            </table>

                            <br />
                            <div style="width: 100%; height: 450px; overflow: scroll">


                                <asp:GridView ID="gv_ProductCode" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                    AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="PRODUCT_CODE_PAGEINDEX_CAHNGE"
                                    OnSelectedIndexChanging="CODE_SELECTED_INDEX_CAHNGE">
                                    <Columns>

                                        <asp:BoundField DataField="T41TYP" HeaderText="Type" ReadOnly="True" SortExpression="P10VEN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="T41COD" HeaderText="Code" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                            <ItemStyle Width="90px" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="T41DES" HeaderText="Description" ReadOnly="True" SortExpression="P10NAM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
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
            </td>
        </tr>
    </table>
</asp:Content>
