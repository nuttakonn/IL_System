<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="ManageCampaign.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Campaign.ManageCampaign" %>

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
    <script type="text/javascript" src="../../../Js/ValidateNumber.js"></script>
    <script type="text/javascript">
        function dataListNote(dataTextss) {
            //debugger;
            document.getElementById("dataListSelected").innerHTML = dataTextss;
        }
    </script>
    <script type="text/javascript">
        window.onresize = function () {
            document.getElementById('myiframeLoadCampaign').height = getClientHeight() - 100;
            document.getElementById('myiframeLoadCampaign').width = getClientWidth() - 20;
        }
    </script>
    <script type="text/javascript">
        function EnableButton(obj) {
            document.getElementById('<%=btnMainDetail_2.ClientID%>').EnableButton = obj;
        }
    </script>
    <script type="text/javascript">
        function EnableButton(obj) {
            document.getElementById('<%=btnMainDetail.ClientID%>').EnableButton = obj;
        }
    </script>
    <script type="text/javascript">
        function ValidateRateTermProductRates(input) {
            //debugger;
            var num = parseFloat(input.value);
            var vBaseRate = parseFloat(document.getElementById('<%= txtCreateCampaingTypeVendorBaseRate.ClientID %>')).value; //txtCreateCampaingTypeVendorBaseRate.GetValue();
            var minRate = <%=this._MinInterestRate%> ;
            //var vBaseRate = document.getElementById('txtCreateCampaingTypeVendorBaseRate');
            //console.log(num + ' - Base ' + vBaseRate + ' _ min ' + minRate);
            var rte = 0.00;
            if (isNaN(num)) { rte = 0.00; }
            else if (num > vBaseRate) { rte = vBaseRate; }
            else if (num < minRate) { rte = minRate; }
            else { rte = addCommas(num.toFixed(2)); }

            //debugger;
            input.value = rte;
            var grid = document.getElementById('<%= gvViewTermOfCampaign.ClientID %>');
            var rowIndex = input.parentNode.parentNode.rowIndex;
            var ri = parseInt(rowIndex);
            var INTrte = grid.rows[ri].cells[7].children[0];
            var CRRrte = grid.rows[ri].cells[8].children[0];
            var INTavg = grid.rows[ri].cells[11];
            var CRRavg = grid.rows[ri].cells[12];

            if (rte > 0.68) {
                INTrte.value = 0.68;
                CRRrte.value = (rte - 0.68).toFixed(2);
                INTavg.innerText  = 0.68;
                CRRavg.innerText  = (rte - 0.68).toFixed(2);
            }
            else
            {
                INTrte.value = rte;
                CRRrte.value = 0.00;
                INTavg.innerText  = rte.toFixed(2);
                CRRavg.innerText  = "0.00";
            }
        }

        function ValidateRateTermProductCRR(input) {
            //debugger;
            var num = parseFloat(input.value);
            var vBaseRate = parseFloat(document.getElementById('<%= txtCreateCampaingTypeVendorBaseRate.ClientID %>')).value; //txtCreateCampaingTypeVendorBaseRate.GetValue();
            var minRate = <%=this._MinInterestRate%>; 
            var rte = 0.00;
            if (isNaN(num)) { rte = 0.00; }
            else if (num > vBaseRate) { rte = vBaseRate; }
            else if (num < minRate) { rte = minRate; }
            else { rte = addCommas(num.toFixed(2)); }

            //input.value = rte;
            
            var grid = document.getElementById('<%= gvViewTermOfCampaign.ClientID %>');
            var rowIndex = input.parentNode.parentNode.rowIndex;
            var ri = parseInt(rowIndex);
            var INTrte = grid.rows[ri].cells[7].children[0];
            //var CRRrte = grid.rows[ri].cells[8].children[0];
            var BSErte = grid.rows[ri].cells[10].children[0];
            var INTavg = grid.rows[ri].cells[11];
            var CRRavg = grid.rows[ri].cells[12];
            var bser = parseFloat(BSErte.value);

            rte = parseFloat(rte);
            if (rte > bser) {
                INTrte.value = 0.00;
                input.value = rte;
                INTavg.innerText  = 0.00;
                CRRavg.innerText  = rte;
                BSErte.value = rte;
            }
            else if (bser - rte < 0.69) {
                INTrte.value = (bser - rte).toFixed(2);
                input.value = rte;
                INTavg.innerText  = (bser - rte).toFixed(2);
                CRRavg.innerText  = rte;
            }
            else
            {
                INTrte.value = 0.00;
                input.value = rte;
                INTavg.innerText  = "0.00";
                CRRavg.innerText  = rte.toFixed(2);
            }

            chkNum(BSErte);
            chkNum(INTavg);
            chkNum(INTrte);
            chkNum(CRRavg);
        }

        function ValidateRateTermProductINT(input) {
            //debugger;
            var num = parseFloat(input.value);
            var vBaseRate = parseFloat(document.getElementById('<%= txtCreateCampaingTypeVendorBaseRate.ClientID %>')).value; //txtCreateCampaingTypeVendorBaseRate.GetValue();
            var minRate = <%=this._MinInterestRate%>; 
            var rte = 0.00;
            if (isNaN(num)) { rte = 0.00; }
            else if (num > vBaseRate) { rte = vBaseRate; }
            else if (num < minRate) { rte = minRate; }
            else { rte = addCommas(num.toFixed(2)); }

            //input.value = rte;
            
            var grid = document.getElementById('<%= gvViewTermOfCampaign.ClientID %>');
            var rowIndex = input.parentNode.parentNode.rowIndex;
            var ri = parseInt(rowIndex);
            //var INTrte = grid.rows[ri].cells[7].children[0];
            var CRRrte = grid.rows[ri].cells[8].children[0];
            var BSErte = grid.rows[ri].cells[10].children[0];
            var INTavg = grid.rows[ri].cells[11];
            var CRRavg = grid.rows[ri].cells[12];
            var bser = parseFloat(BSErte.value);

            rte = parseFloat(rte);
            if (rte > bser) {
                input.value = rte;
                CRRrte.value = 0.00;
                INTavg.innerText  = rte;
                CRRavg.innerText  = 0.00;
                BSErte.value = rte;
            }
            else if (bser - rte < 0.69) {
                input.value = rte;
                CRRrte.value = (bser - rte).toFixed(2);
                INTavg.innerText  = rte;
                CRRavg.innerText  = (bser - rte).toFixed(2);
            }
            else
            {
                input.value = rte;
                CRRrte.value = 0.00;
                INTavg.innerText  = rte.toFixed(2);
                CRRavg.innerText  = "0.00";
            }

            chkNum(BSErte);
            chkNum(INTavg);
            chkNum(CRRrte);
            chkNum(CRRavg);
        }

        function ValidateRateTermProductINF(input) {
            //debugger;
            var num = parseFloat(input.value);
            var vBaseRate = parseFloat(document.getElementById('<%= txtCreateCampaingTypeVendorBaseRate.ClientID %>')).value; //txtCreateCampaingTypeVendorBaseRate.GetValue();
            var minRate = parseFloat(-1);
            var rte = 0.00;
            if (isNaN(num)) { rte = 0.00; }
            else if (num > 0) { rte = 0.00; }
            else if (num < minRate) { rte = minRate; }
            else { rte = addCommas(num.toFixed(2)); }

            input.value = rte;
        }

        function gvViewTermOfCampaignOnFocusIn(input, tbName) {
            var num = parseFloat(input.value);
            var rowIndex = input.parentNode.parentNode.rowIndex;
            var hdRowName = document.getElementById('<%= hdTermOfCampaignRowName.ClientID %>');
            var hdRowValue = document.getElementById('<%= hdTermOfCampaignRowValue.ClientID %>');
            var hdRowIndex = document.getElementById('<%= hdTermOfCampaignRowIndex.ClientID %>');

            hdRowName.value = tbName;
            hdRowValue.value = num.toFixed(2);
            hdRowIndex.value = rowIndex;
            input.value = "";
        }

        function gvViewTermOfCampaignOnFocusOut(input, tbName) {
            var num = parseFloat(input.value);
            var rowIndex = input.parentNode.parentNode.rowIndex;
            var hdRowName = document.getElementById('<%= hdTermOfCampaignRowName.ClientID %>');
            var hdRowValue = document.getElementById('<%= hdTermOfCampaignRowValue.ClientID %>');
            var hdRowIndex = document.getElementById('<%= hdTermOfCampaignRowIndex.ClientID %>');

            if (tbName == hdRowName.value) {
                if (rowIndex == hdRowIndex.value) {
                    if (!num) {
                        input.value = hdRowValue.value;
                    }
                }
                hdRowName.value = "";
                hdRowValue.value = "";
                hdRowIndex.value = "";
            }
        }
    </script>
    <table>
        <tr>
            <td></td>
            <td></td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" HeaderText=""
                    Width="100%" Height="136px">
                    <ContentPaddings Padding="5px" />
                    <PanelCollection>
                        <dx:PanelContent runat="server" ID="panelStatusCampaign">
                            <table id="CampaignStatus" runat="server" width="100%" cellpadding="10" cellspacing="0" style="margin-top: 5px; font-family: Tahoma; font-size: small; font-weight: 100; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                <tr style="vertical-align: middle;">
                                    <td align="left">
                                        <asp:Image ID="imgNewCampaignOn" runat="server" ImageUrl="~/Images/icon/status/lightBlue.gif" />
                                        <asp:Image ID="imgNewCampaignOff" runat="server" ImageUrl="~/Images/icon/status/grey.gif" />
                                        New Campaign&emsp;&emsp;
                                       <asp:Image ID="imgActiveCampaignOn" runat="server" ImageUrl="~/Images/icon/status/lightGreen.gif" />
                                        <asp:Image ID="imgActiveCampaignOff" runat="server" ImageUrl="~/Images/icon/status/grey.gif" />
                                        Active Campaign&emsp;&emsp;
                                       <asp:Image ID="imgEndCampaignOn" runat="server" ImageUrl="~/Images/icon/status/lightRed.gif" />
                                        <asp:Image ID="imgEndCampaignOff" runat="server" ImageUrl="~/Images/icon/status/grey.gif" />
                                        End Campaign
                                    </td>
                                    <td>
                                        <%--  อัตราดอกเบี้ยที่กำหนด: .. Cru.Rate: .. inf.Rate: .. วันที่เริ่มใช้: .. วันสิ้นสุดการใช้--%>
                                    </td>
                                    <td align="right">
                                        <asp:CheckBoxList ID="chkShowMode" runat="server" OnClick="return false;" Style="margin-right: -10px;">
                                            <asp:ListItem Text="" Value="" Selected="true"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td align="left" width="100">
                                        <asp:Label ID="lblCampaignMode" runat="server" Style="margin-left: -10px;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table align="left" width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa; height: auto;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr id="Mode" runat="server">
                                                <td align="left" width="250px" height="24px">
                                                    <asp:Label ID="Label27" runat="server" Text="Mode"></asp:Label>
                                                </td>
                                                <td align="left" height="24px">
                                                    <asp:RadioButtonList ID="rdoCreateCampaign" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoCreateCampaign_SelectedIndexChanged">
                                                        <asp:ListItem Text="Create Campaign" Value="CC"></asp:ListItem>
                                                        <asp:ListItem Text="Load Data from Other Campaign" Value="LC"></asp:ListItem>
                                                    </asp:RadioButtonList>

                                                </td>
                                                <td></td>


                                            </tr>
                                            <tr id="SelectCampaignType" runat="server" visible="false">
                                                <td align="left" width="250px" height="24px">
                                                    <asp:Label ID="lblSelectCampaign" runat="server" Text="Campaign Type"></asp:Label>
                                                </td>
                                                <td align="left" height="24px" style="padding-left: 5px;">
                                                    <asp:DropDownList ID="ddlCampaignType" runat="server" Height="20" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlCampaignType_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True">Select</asp:ListItem>
                                                        <asp:ListItem Value="MAKER">Maker</asp:ListItem>
                                                        <asp:ListItem Value="VENDOR">Vendor</asp:ListItem>
                                                        <asp:ListItem Value="ESB">ESB</asp:ListItem>
                                                        <asp:ListItem Value="SHARESUB">Share Sub
                                                        </asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>


                                            </tr>

                                            <asp:Panel ID="pCampaignDetail" runat="server" Enabled="false">
                                                <tr id="Maker" runat="server" visible="false">
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label2" runat="server" Text="Campaign for Maker Code" Width="170px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtMakerCode" runat="server" Width="170px" Enabled="false"></asp:TextBox>
                                                        &nbsp;
                                                            <asp:ImageButton OnClick="btnAddMakerCodeClick" Height="18" ID="btnAddMakerCode" runat="server" AlternateText="ImageButton" ToolTip="Find Maker" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" CausesValidation="false" />
                                                        <asp:RequiredFieldValidator ID="ReqtxtMakerCode" runat="server" ErrorMessage="* Required." ForeColor="Red" ControlToValidate="txtMakerCode" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>

                                                <tr id="Vendor" runat="server" visible="false">
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="lblVendorCode" runat="server" Text="Campaign for Vendor Code"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtVendorCode" runat="server" Width="170px" Enabled="false"></asp:TextBox>
                                                        &nbsp;
                                                            <asp:ImageButton OnClick="btnAddVendorClick" Height="18" ID="btnAddVendor" runat="server" AlternateText="ImageButton" ToolTip="Find Vendor" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" CausesValidation="false" />
                                                        <asp:HiddenField ID="hdfLoanType" runat="server" />
                                                        <asp:RequiredFieldValidator ID="ReqtxtVendorCode" runat="server" ErrorMessage="* Required." ForeColor="Red" ControlToValidate="txtVendorCode" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="HdnIsEdit1" runat="server" />
                                                        <asp:HiddenField ID="HdnIsEdit2" runat="server" />
                                                        <asp:HiddenField ID="ds_popup_vendor" runat="server" />
                                                        <asp:HiddenField ID="ds_vendor_list" runat="server" />
                                                        <asp:HiddenField ID="ds_term_of_campaign" runat="server" />
                                                        <asp:HiddenField ID="ds_term_of_campaign_detail" runat="server" />
                                                        <asp:HiddenField ID="ds_AddItemProduct" runat="server" />
                                                        <asp:HiddenField ID="ds_AddItemExceptProduct" runat="server" />
                                                        <asp:HiddenField ID="ds_itemSeleced" runat="server" />
                                                        <asp:HiddenField ID="ds_gv_selectlistproduct" runat="server" />
                                                        <asp:HiddenField ID="ds_DS_ILCP04" runat="server" />
                                                        <asp:HiddenField ID="ds_gv_selectlistvendor" runat="server" />
                                                        <asp:HiddenField ID="ds_gridBanch" runat="server" />
                                                        <asp:HiddenField ID="ds_initial_value" runat="server" />
                                                        <asp:HiddenField ID="ds_gridAppType" runat="server" />
                                                        <asp:HiddenField ID="ds_gvListVendor_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvListVendor" runat="server" />
                                                        <asp:HiddenField ID="dt_Popup_Vendorlistselected_2" runat="server" />
                                                        <asp:HiddenField ID="dt_Popup_Vendorlistselected_1" runat="server" />
                                                        <asp:HiddenField ID="ds_gvCampaign_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvProductType_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvProductCode_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvModel_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvProductItem_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvVendor_2" runat="server" />
                                                        <asp:HiddenField ID="ds_gvVendor" runat="server" />
                                                        <asp:HiddenField ID="ds_gvProductType" runat="server" />
                                                        <asp:HiddenField ID="ds_gvProductCode" runat="server" />
                                                        <asp:HiddenField ID="ds_gvModel" runat="server" />
                                                        <asp:HiddenField ID="ds_gvProductItem" runat="server" />
                                                        <asp:HiddenField ID="ds_gvCampaign" runat="server" />
                                                        <asp:HiddenField ID="ds_gvListProduct" runat="server" />
                                                        <asp:HiddenField ID="dt_Popup_Productlistselected" runat="server" />
                                                        <asp:HiddenField ID="hd_SaveNote" runat="server" />
                                                    </td>
                                                </tr>c

                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label1" runat="server" Text="Campaign Code" Width="170px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtCampaignCode" runat="server" Width="170px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label5" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                            Text="Campaign Name" Width="170px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    " style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtCampaignName" runat="server" Width="530px" MaxLength="200"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="ReqtxtCampaignName" runat="server" Display="Dynamic" ErrorMessage="* Required." ForeColor="Red" ControlToValidate="txtCampaignName" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label99" runat="server" Text="Product Detail" Width="170px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtProductDetail" runat="server" Width="530px" MaxLength="150"></asp:TextBox>&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label100" runat="server" Text="Special Premium" Width="170px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="padding-left: 5px;">
                                                        <asp:TextBox ID="txtSpecialPremium" runat="server" Width="530px" MaxLength="150"></asp:TextBox>&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label6" runat="server" Text="Start Date"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <table width="600px">
                                                            <tr>
                                                                <td align="left" width="200px" height="24px">
                                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="92px" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/eeee"
                                                                        PopupButtonID="ImageButton1" TargetControlID="txtStartDate">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="ReqtxtStartDate" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="txtStartDate" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:Label ID="Label8" runat="server" Text="End Date" Style="margin-left: -3px;"></asp:Label>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="92px" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged" Style="margin-left: -3px;"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd/MM/eeee"
                                                                        PopupButtonID="ImageButton1" TargetControlID="txtEndDate">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="ReqtxtEndDate" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="txtEndDate" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="left" width="70px" height="24px">
                                                                    <asp:Label ID="Label4" runat="server" Text="X-Due"></asp:Label>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <%--<dx:A ID="txtXDue" runat="server" Width="50px" TextMode="Number" Text="1" MaxLength="1"></dx:A>--%>
                                                                    <dx:ASPxSpinEdit ID="txtXDue" runat="server" Width="50px" NumberType="Integer"></dx:ASPxSpinEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label7" runat="server" Text="Closing Application"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="600px">
                                                            <tr>
                                                                <td align="left" width="200px" height="24px">
                                                                    <asp:TextBox ID="txtClosingApplicationDate" runat="server" Width="92px" AutoPostBack="true" OnTextChanged="txtClosingApplicationDate_TextChanged"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" Format="dd/MM/eeee"
                                                                        PopupButtonID="ImageButton1" TargetControlID="txtClosingApplicationDate">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="ReqtxtClosingApplicationDate" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="txtClosingApplicationDate" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:Label ID="Label9" runat="server" Text="Closing Lay Bill"></asp:Label>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:TextBox ID="txtClosingLayBillDate" runat="server" Width="92px"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" Format="dd/MM/eeee"
                                                                        PopupButtonID="ImageButton1" TargetControlID="txtClosingLayBillDate">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="ReqtxtClosingLayBillDate" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="txtClosingLayBillDate" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td align="left" width="70px" height="24px">
                                                                    <asp:Label ID="Label11" runat="server" Text="F.Install"></asp:Label>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <dx:ASPxSpinEdit ID="txtFInstall" runat="server" Style="margin-left: -3px;" Width="51px" NumberType="Integer" Enabled="false" SpinButtons-ShowIncrementButtons="False">
                                                                        <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label3" runat="server" Text="Apply Type" Width="170px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="font-weight: 100;">
                                                        <asp:CheckBoxList ID="ckbPoupCreateCampaingTypeVendorApplicationType" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td align="left" width="170px" height="24px">
                                                        <asp:Label ID="Label17" runat="server" Text="Type Sub" Width="150px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="font-weight: 100;">
                                                        <asp:RadioButtonList ID="rdoCreateCampaingTypeVendorTypeSub" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                                            <asp:ListItem Value="R" Text="Rate" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="I" Text="Installment" Enabled="false"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label103" runat="server" Text="Share Sub" Width="150px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="font-weight: 100;">
                                                        <asp:CheckBoxList ID="ckbCreateCampaignTypeVendorShareSub" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ckbCreateCampaignTypeVendorShareSub_SelectedIndexChanged">
                                                            <asp:ListItem Text="Maker" Value="Maker"></asp:ListItem>
                                                            <asp:ListItem Text="Vendor" Value="Vendor"></asp:ListItem>
                                                            <asp:ListItem Text="ESB" Value="ESB"></asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </td>

                                                </tr>
                                                <%--<tr>
                                                        <td align="left" width="250px" height="24px">
                                                            <asp:Label ID="Label4" runat="server" Text="Branch" Width="170px"></asp:Label>
                                                        </td>
                                                        <td align="left" height="24px" style="font-weight: 100;">
                                                            <asp:CheckBoxList ID="ckbPoupCreateCampaingTypeVendorBranch" runat="server" RepeatDirection="Horizontal" RepeatColumns="5">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>--%>
                                            </asp:Panel>
                                            <asp:HiddenField ID="ds_multi_rate" runat="server" />
                                            <asp:HiddenField ID="ds_party_rate" runat="server" />
                                            <asp:Panel ID="pCampaignDetailValue" runat="server" Enabled="false">
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label10" runat="server" Text="Calculate Type" Width="150px"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px" style="font-weight: 100;">
                                                        <asp:RadioButtonList ID="rdoCreateCampaingTypeVendorCalculateType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                            OnSelectedIndexChanged="rdoCreateCampaingTypeVendorCalculateTypeSelectedIndexChanged">
                                                            <asp:ListItem Value="R" Text="R:Base rate"></asp:ListItem>
                                                            <asp:ListItem Value="I" Text="I:Fix rate" Enabled="false"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <asp:RequiredFieldValidator ID="ReqrdoCreateCampaingTypeVendorCalculateType" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="rdoCreateCampaingTypeVendorCalculateType" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>

                                                <tr id="pBaseRate" runat="server" visible="false">
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label20" runat="server" Text="Campaign Support Term Range"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="470px" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="100" height="24px" style="font-weight: 100;">

                                                                    <asp:RadioButtonList ID="rbCreateCampaingTypeVendorCampaignSupport" runat="server" RepeatDirection="Horizontal"
                                                                        OnSelectedIndexChanged="rbCreateCampaingTypeVendorCampaignSupportSelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="Y" Text="YES"></asp:ListItem>
                                                                        <asp:ListItem Value="N" Text="NO"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:RequiredFieldValidator ID="ReqrbCreateCampaingTypeVendorCampaignSupport" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="rbCreateCampaingTypeVendorCampaignSupport" SetFocusOnError="true"></asp:RequiredFieldValidator>

                                                                </td>
                                                                <td width="100" height="24px" align="center">
                                                                    <asp:Label ID="Label21" runat="server" Text="Base Rate"></asp:Label>
                                                                </td>
                                                                <td height="24px">
                                                                    <dx:ASPxTextBox ID="txtCreateCampaingTypeVendorBaseRate" runat="server" ClientInstanceName="txtCreateCampaingTypeVendorBaseRate" Width="80" Height="18" MaxLength="5" AutoPostBack="true" OnTextChanged="txtCreateCampaingTypeVendorBaseRate_TextChanged" OnKeyPress="return chkNumber(this)">

                                                                        <ValidationSettings Display="Dynamic" ValidateOnLeave="false">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>


                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr id="pOptionAddMultiTerm" runat="server" visible="false">
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label55" runat="server" Text="Add Single or Multi Term"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="470px" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td width="100" height="24px" style="font-weight: 100;">
                                                                    <asp:RadioButtonList ID="rbOptionalAddMultiTerm" runat="server" RepeatDirection="Horizontal"
                                                                        OnSelectedIndexChanged="rbOptionalAddMultiTerm_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="S" Text="Single"></asp:ListItem>
                                                                        <asp:ListItem Value="M" Text="Multi"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    <asp:RequiredFieldValidator ID="ReqrbOptionalAddMultiTerm" runat="server" Display="Dynamic" ErrorText="* Required" ForeColor="Red" ControlToValidate="rbOptionalAddMultiTerm" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr id="rSubSeq" runat="server" visible="false">
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label34" runat="server" Text="Share Sub Seq"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <table width="400px" cellpadding="0" cellspacing="0">
                                                            <tr style="height: 30px;">
                                                                <td align="left" width="70px" height="24px">
                                                                    <dx:ASPxTextBox ID="txtSubSeq" runat="server" Width="70px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtSubSeq_TextChanged" ReadOnly="true">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="gSubSeq">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td width="70px" height="24px">
                                                                    <dx:ASPxButton ID="btnAddSubSeq" runat="server" Text="Set" Width="70px" Height="24px" BackColor="#66CCFF" OnClick="btnAddSubSeq_Click" ValidationGroup="gSubSeq">
                                                                        <Image Url="~\Images\icon\add.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <asp:Panel ID="pRangeY" runat="server" Visible="false">
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label12" runat="server" Text="Range by Term"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="400px" cellpadding="0" cellspacing="0">
                                                            <tr style="height: 30px;">
                                                                <td align="left" width="150px" height="24px">
                                                                    <asp:Label ID="Label51" runat="server" Text="Add Total Term" Width="150px"></asp:Label>
                                                                </td>
                                                                <td align="left" width="70px" height="24px">
                                                                    <dx:ASPxTextBox ID="txt_addTerm" runat="server" Width="70px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txt_addTerm_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="MultiRate">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <%--<td width="10px"></td>--%>
                                                                <td align="left" width="100px" height="24px">
                                                                    <asp:Label ID="Label52" runat="server" Text="Month"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 30px;">
                                                                <td align="left" width="150px" height="24px"><%--style="font-weight: 100;">--%>
                                                                    <asp:Label ID="Label24" runat="server" Text="Term Range"></asp:Label>
                                                                    <%--<asp:RadioButtonList ID="rbRange" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="rbRangeSelectedIndexChanged">
                                                                                <asp:ListItem Value="1" Text="1" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                            </asp:RadioButtonList>--%>
                                                                    <%--<asp:RequiredFieldValidator ID="ReqrbRange" runat="server" ErrorMessage="* Required." ForeColor="Red" ControlToValidate="rbRange" SetFocusOnError="True" Display="Dynamic" ValidationGroup="MultiRate"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                                <td align="left" width="70px" height="24px" style="font-weight: 100;">
                                                                    <dx:ASPxTextBox ID="txtTermRange" runat="server" Width="70px" ReadOnly="true" MaxLength="5" AutoPostBack="True" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtTermRange_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="MultiRate">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 30px;">
                                                                <td align="left" width="150px" height="24px">
                                                                    <asp:Label ID="lbEndTermRange" runat="server" Text="Total Term of Range 1" Width="150px"></asp:Label>
                                                                </td>
                                                                <%--<td width="10px"></td>--%>
                                                                <td width="70px" height="24px" style="text-align: left; font-family: Tahoma; font-size: small;">
                                                                    <div style="float: left;">
                                                                        <dx:ASPxTextBox ID="txtEndTermRange" runat="server" Width="70px" MaxLength="5" AutoPostBack="True" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtEndTermRange_TextChanged">
                                                                            <ValidationSettings Display="Dynamic" ValidationGroup="MultiRate">
                                                                                <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </div>
                                                                </td>
                                                                <%--<div style="float: left;">
                                                                                &nbsp;&nbsp;
                                                                            </div>--%>
                                                            </tr>
                                                            <tr style="height: 30px;">
                                                                <td></td>
                                                                <td width="70px" height="24px">
                                                                    <dx:ASPxButton ID="btnSetTermRange" runat="server" Text="Add" Width="70px" Height="24px" BackColor="#66CCFF" OnClick="btnSetTermRangeClick" ValidationGroup="MultiRate">
                                                                        <Image Url="~\Images\add.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td width="70px" height="24px">
                                                                    <dx:ASPxButton ID="btnClearTermRange" runat="server" Text="Clear" Width="70px" Height="24px" BackColor="#66CCFF" OnClick="btnClearTermRangeClick" CausesValidation="false">
                                                                        <Image Url="~\Images\refresh.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:Panel>

                                            <asp:Panel ID="pAddMonth" runat="server" Visible="false">
                                                <%--<tr>
                                                            <td align="left" width="250px" height="24px">
                                                                <asp:Label ID="Label26" runat="server" Text="Add Term" Width="100px"></asp:Label>
                                                            </td>
                                                            <td align="left" height="24px">
                                                                <table width="500px" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" width="100px" height="24px">
                                                                            <dx:ASPxTextBox ID="txt_addTerm" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumber(this)">
                                                                                <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                                    <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                                                                        </td>
                                                                        <td align="left" width="120px" height="24px">
                                                                            <asp:Label ID="Label25" runat="server" Text="Month"></asp:Label>
                                                                        </td>
                                                                        <td align="left" width="150px" height="24px">&nbsp;
                                                                        </td>
                                                                        <td align="left" width="100px" height="24px">&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>--%>
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="lbStartTerm" runat="server" Text="Start Term"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="500px">
                                                            <tr>
                                                                <td align="left" width="100px" height="24px">
                                                                    <dx:ASPxTextBox ID="txtStartTerm" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtStartTerm_TextChanged" Enabled="false">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:Label ID="lbEndTerm" runat="server" Text="End Term" Width="120px">
                                                                    </asp:Label>
                                                                </td>
                                                                <td align="left" width="150px" height="24px">
                                                                    <dx:ASPxTextBox ID="txtEndTerm" runat="server" Width="70px" Height="24px" Enabled="false" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtEndTerm_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td align="left" width="100px" height="24px">&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:Panel>

                                            <asp:Panel ID="pAddMultiTerm" runat="server" Visible="false">
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label58" runat="server" Text="Input Multi Range"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="700px">
                                                            <tr>
                                                                <td align="left" width="150px" height="24px" colspan="3">
                                                                    <dx:ASPxTextBox ID="txtTermMultiRange" runat="server" Width="150px" Height="24px" MaxLength="50" AutoPostBack="true" OnKeyPress="return chkNumberMultiTerm(this)">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td align="left" width="150px" height="24px" colspan="8">
                                                                    <asp:Label ID="Label59" runat="server" Text="(Ex. 3,6,9-48)" ForeColor="GrayText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:Panel>

                                            <asp:Panel ID="pAddRate" runat="server" Visible="false">
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label13" runat="server" Text="Rate of Range"></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="700px">
                                                            <tr>
                                                                <td align="left" width="100px" height="24px">
                                                                    <asp:Label ID="Label40" runat="server" Text="Customer Rate"></asp:Label>
                                                                </td>
                                                                <td align="left" width="100px" height="24px" colspan="3">
                                                                    <dx:ASPxTextBox ID="txtRateforRange" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberMinus(this)" OnTextChanged="txtRateforRange_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="rSubVendor" runat="server">
                                                                <td align="left" width="170px" height="24px">
                                                                    <asp:Label ID="Label22" runat="server" Text="Sub rate Vendor"></asp:Label>
                                                                </td>
                                                                <td align="left" width="100px" height="24px" colspan="3">
                                                                    <dx:ASPxTextBox ID="txtSubRateVendor" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumber(this)" OnTextChanged="txtSubRateVendor_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="rSubMaker" runat="server">
                                                                <td align="left" width="170px" height="24px">
                                                                    <asp:Label ID="Label39" runat="server" Text="Sub rate Maker"></asp:Label>
                                                                </td>
                                                                <td align="left" width="100px" height="24px" colspan="3">
                                                                    <dx:ASPxTextBox ID="txtSubRateMaker" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumber(this)" OnTextChanged="txtSubRateMaker_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="rSubESB" runat="server">
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:Label ID="Label23" runat="server" Text="Sub rate ESB">
                                                                    </asp:Label>
                                                                </td>
                                                                <td align="left" width="100px" height="24px" colspan="3">
                                                                    <dx:ASPxTextBox ID="txtSubRateESB" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumber(this)" OnTextChanged="txtSubRateESB_TextChanged">
                                                                        <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="130px" height="24px">
                                                                    <dx:ASPxButton ID="btnInsertTerm" runat="server" Text="Insert Term" Width="120px" Height="24px" BackColor="#66CCFF" ValidationGroup="InsertTerm" OnClick="btnInsertTerm_Click">
                                                                        <Image Url="~\Images\icon\success.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>

                                                        </table>

                                                    </td>
                                                </tr>
                                            </asp:Panel>

                                            <asp:Panel ID="pRangeN" runat="server" Visible="false">
                                                <tr>
                                                    <td align="left" width="250px" height="24px">
                                                        <asp:Label ID="Label15" runat="server" Text="Rate Seq.(%)"></asp:Label>
                                                        <asp:Label ID="Label18" runat="server" Text="1."></asp:Label>
                                                    </td>
                                                    <td align="left" height="24px">
                                                        <table width="500px">
                                                            <tr>
                                                                <td align="left" width="100px" height="24px">
                                                                    <dx:ASPxTextBox ID="txtCreateCampaignRateSeq" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnTextChanged="txtCreateCampaignRateSeq_TextChanged" OnKeyPress="return chkNumber(this)">
                                                                        <ValidationSettings Display="Dynamic">
                                                                            <%--<RegularExpression ErrorText="Only numeric allowed." ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></RegularExpression>--%>
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td align="left" width="120px" height="24px">
                                                                    <asp:Label ID="Label16" runat="server" Text="Total Term/Rate">
                                                                    </asp:Label>
                                                                </td>
                                                                <td align="left" width="250px" height="24px">
                                                                    <dx:ASPxTextBox ID="txtCreateCampaignTotalTermRate" runat="server" Width="70px" Height="24px" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)">
                                                                        <ValidationSettings Display="Dynamic">
                                                                            <%--<RegularExpression ErrorText="Only numeric allowed." ValidationExpression="^[1-9]\d$"></RegularExpression>--%>
                                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <%--<td align="left" width="60px" height="24px">
                                                                            <asp:Label ID="Label47" runat="server" Text="To">
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td align="left" width="250px" height="24px">
                                                                            <dx:ASPxTextBox ID="txtCreateCampaignTotalTermRateTo" runat="server" Width="70px" Height="24px" AutoPostBack="true" OnKeyPress="return chkNumber(this)">
                                                                                <ValidationSettings Display="Dynamic">
                                                                                  <RegularExpression ErrorText="Only numeric allowed." ValidationExpression="^[1-9]\d$"></RegularExpression>
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                                                                        </td>--%>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                        </table>
                                        <table width="100%">
                                            <asp:Panel ID="pGridMultiRate" runat="server" Visible="false">

                                                <tr>
                                                    <td colspan="2">
                                                        <hr style="margin-top: 10px; margin-bottom: 10px;" />
                                                        <asp:GridView ID="gvMultiRate" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" AllowPaging="true" PageSize="10"
                                                            EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                            ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                                            OnPageIndexChanging="gvMultiRate_PageIndexChanging" OnSelectedIndexChanging="gvMultiRate_SelectedIndexChanging">
                                                            <%--OnPageIndexChanging="gvPartyRatePageIndexChanging">--%>
                                                            <Columns>
                                                                <asp:ButtonField CommandName="Select" HeaderText="Add Share Sub" ButtonType="Image" Visible="false"
                                                                    ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    ValidationGroup="false">
                                                                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                                                    <ItemStyle Width="10px" Height="10px" HorizontalAlign="Center" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField DataField="RATES" HeaderText="Rate" SortExpression="RATES">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C05STO" HeaderText="Total Term" SortExpression="C05STO">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C05CSQ" HeaderText="SubSeq" SortExpression="C05CSQ">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C05RSQ" HeaderText="Seq" SortExpression="C05RSQ">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C05STM" HeaderText="Term" SortExpression="C05STM">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="C05SBT" HeaderText="Type" SortExpression="C05SBT">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Easybuy" HeaderText="Easybuy" SortExpression="Easybuy">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ESubRate" HeaderText="E.Sub Rate" SortExpression="ESubRate">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Maker" HeaderText="Maker" SortExpression="Maker">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MSubRate" HeaderText="M.Sub Rate" SortExpression="MSubRate">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VSubRate" HeaderText="V.Sub Rate" SortExpression="VSubRate">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CampaignStartTerm" HeaderText="CampaignStartTerm" SortExpression="CampaignStartTerm">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CampaignEndTerm" HeaderText="CampaignEndTerm" SortExpression="CampaignEndTerm">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <FooterStyle BackColor="#CCCC99" />
                                                            <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                            <RowStyle BackColor="#F7F7DE" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>

                                            </asp:Panel>
                                            <%-- </asp:Panel>--%>
                                            <asp:Panel ID="pButtonCreate" runat="server">
                                                <tr id="buttonTempCreate" runat="server">
                                                    <td width="250px"></td>
                                                    <td align="left">
                                                        <table width="150px" align="left" cellpadding="5" style="margin-left: -3px;">
                                                            <tr>
                                                                <td width="50%" align="left">
                                                                    <dx:ASPxButton ID="btnCreateCampaignOK" runat="server" Text="OK" Height="25" Width="70px" Cursor="pointer" OnClick="btnCreateCampaignOKClick" Enabled="false" UseSubmitBehavior="false" AutoPostBack="true">
                                                                        <Image Url="~\Images\icon\success.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td width="50%" align="left">
                                                                    <dx:ASPxButton ID="btnCreateCampaignClear" runat="server" Text="Clear" Height="25" Width="70px" Cursor="pointer" CausesValidation="false" OnClick="btnCreateCampaignClearClick">
                                                                        <Image Url="~\Images\icon\refresh.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr id="SaveButton" runat="server" visible="false">
                                                    <td width="250px">&nbsp;</td>
                                                    <td>
                                                        <table width="300px" align="left" cellpadding="5" style="margin-left: -3px;">
                                                            <tr>
                                                                <td width="30%" align="left">
                                                                    <dx:ASPxButton ID="btnSave" runat="server" Text="Save" AutoPostBack="false" Height="25" Width="70px" Cursor="pointer" OnClick="BTN_CONFIRM_SAVE">
                                                                        <Image Url="~\Images\icon\save.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td width="30%" align="left">
                                                                    <dx:ASPxButton ID="btnClear" runat="server" Text="Clear" Height="25" Width="70px" Cursor="pointer" OnClick="btnClearClick">
                                                                        <Image Url="~\Images\icon\refresh.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td width="40%" align="left">
                                                                    <dx:ASPxButton ID="btnClearRate" runat="server" Cursor="pointer" Height="25px" OnClick="btnClearRateClick" Text="CLEAR RATE" Width="150px">
                                                                        <Image Url="~\Images\icon\deletenew.png" Width="16px"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr id="UpdateButton" runat="server" visible="false">
                                                    <td width="250px">&nbsp;</td>
                                                    <td>
                                                        <table width="300px" align="left" cellpadding="5">
                                                            <tr>
                                                                <td width="30%" align="left">
                                                                    <dx:ASPxButton ID="btnUpdate" runat="server" Text="Update" Height="25" Width="70px" Cursor="pointer" OnClick="btnUpdateClick">
                                                                        <Image Url="~\Images\icon\success.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr id="DeleteButton" runat="server" visible="false">
                                                    <td width="250px">&nbsp;</td>
                                                    <td>
                                                        <table width="300px" align="left" cellpadding="5">
                                                            <tr>
                                                                <td width="30%" align="left">
                                                                    <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" Height="25" Width="70px" Cursor="pointer" OnClick="btnDeleteClick">
                                                                        <Image Url="~\Images\icon\delete.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="EditButton" runat="server" visible="false">
                                                    <td width="250px">&nbsp;</td>
                                                    <td>
                                                        <table width="300px" align="left" cellpadding="5">
                                                            <tr>
                                                                <td width="30%" align="left">
                                                                    <dx:ASPxButton ID="btnEditCampaign" runat="server" Text="Edit" Height="25" Width="70px" Cursor="pointer" OnClick="BTN_CONFIRM_EDIT_CAMPAIGN">
                                                                        <%--    <dx:ASPxButton ID="btnEditCampaign" runat="server" Text="Edit" Height="25" Width="70px" Cursor="pointer" OnClick="BTN_CONFIRM_SAVE">--%>
                                                                        <Image Url="~\Images\icon\edit.png" Width="16"></Image>
                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                        </DisabledStyle>
                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show(); }" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pTab" runat="server">
                                            <%--Start Contain tab panel--%>
                                            <dx:ASPxPageControl Width="100%" Height="100%" ID="tabDetail" runat="server" ActiveTabIndex="2"
                                                EnableHierarchyRecreation="True" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css"
                                                CssPostfix="Office2003Blue" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" AutoPostBack="true"
                                                ActivateTabPageAction="Click" LoadingPanelImagePosition="Top" OnActiveTabChanged="tabDetail_ActiveTabChanged">
                                                <TabPages>

                                                    <dx:TabPage Text="Fix Installment" Name="FixInstallment">
                                                        <TabStyle Font-Bold="True">
                                                        </TabStyle>
                                                        <ContentCollection>
                                                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">

                                                                <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="gvInstallment" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                AllowPaging="True" EmptyDataText="No Records Found" ShowHeaderWhenEmpty="true"
                                                                                OnPageIndexChanging="gvInstallmentPageIndexChanging">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Installment" HeaderText="Installment(Baht)">
                                                                                        <ItemStyle Width="80px" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="PrincipleAMT" HeaderText="Principle AMT(Baht)">
                                                                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="InstallmentTerm" HeaderText="Installment Term">
                                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="AdjustmentTerm" HeaderText="Term(Month) Adj.Term">
                                                                                        <ItemStyle Width="80px" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="CustomerInterestRate" HeaderText="Customer Interest Rate">
                                                                                        <ItemStyle Width="80px" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="CustomerCreditUsage" HeaderText="Customer Credit Usage">
                                                                                        <ItemStyle Width="80px" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="InterestRateMaker" HeaderText="Interest Rate Maker">
                                                                                        <ItemStyle Width="80px" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="TotalRate" HeaderText="Total Rate">
                                                                                        <ItemStyle Width="80px" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                                <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
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
                                                                </table>
                                                                <br />

                                                                <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                    <tr>
                                                                        <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                            <asp:Label ID="Label42" runat="server" Text="Product List"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="gvProductList" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                AllowPaging="True" EmptyDataText="No Records Found" ShowHeaderWhenEmpty="true"
                                                                                OnPageIndexChanging="gvProductListPageIndexChanging">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Code" HeaderText="Code">
                                                                                        <ItemStyle Width="150px" />
                                                                                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name">
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                                <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
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
                                                                </table>
                                                            </dx:ContentControl>
                                                        </ContentCollection>
                                                    </dx:TabPage>

                                                    <dx:TabPage Text="Term | Product" Name="TermProduct">
                                                        <TabStyle Font-Bold="True">
                                                        </TabStyle>
                                                        <ContentCollection>
                                                            <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                                                                <asp:HiddenField ID="hdTermOfCampaignRowName" runat="server" />
                                                                <asp:HiddenField ID="hdTermOfCampaignRowValue" runat="server" />
                                                                <asp:HiddenField ID="hdTermOfCampaignRowIndex" runat="server" />
                                                                <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                    <tr>
                                                                        <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                            <asp:Label ID="Label19" runat="server" Text="View Term of Campaign"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="100%" height="24px" style="font-weight: 100;">
                                                                            <div style="height: 145px; width: auto; overflow-x: auto; overflow-y: auto; background-color: #F7F7DE; margin-top: 5px; width: auto">
                                                                                <asp:GridView ID="gvViewTermOfCampaign" runat="server" AutoGenerateColumns="False"
                                                                                    BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                    EmptyDataText="No Records Found" ShowHeaderWhenEmpty="true"
                                                                                    OnSelectedIndexChanging="GV_SELECTED_LITSUB_SEQ">
                                                                                    <Columns>
                                                                                        <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ValidationGroup="false">
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                                                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                                        </asp:ButtonField>
                                                                                        <asp:BoundField DataField="C02TTM" HeaderText="Total Term" SortExpression="C02TTM">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="C02CSQ" HeaderText="Sub Seq." SortExpression="C02CSQ">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="C02RSQ" HeaderText="Seq." SortExpression="C02RSQ">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="C02TTR" HeaderText="M./Range" SortExpression="C02TTR">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="C02FMT" HeaderText="Begin" SortExpression="C02FMT">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="C02TOT" HeaderText="End" SortExpression="C02TOT">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="INT.Rate" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="tbC02INR" runat="server" Text='<%# Bind("C02INR") %>' Style="border-color: #F0F0F0; text-align: center; border-radius: 5px;" Width="70" Height="18" onfocusin="JavaScript:gvViewTermOfCampaignOnFocusIn(this, 'tbC02INR')" onfocusout="JavaScript:gvViewTermOfCampaignOnFocusOut(this, 'tbC02INR')" OnChange="JavaScript:ValidateRateTermProductINT(this); chkNum(this);" OnKeyPress="return chkNumberMinus(this)" OnTextChanged="tbC02INR_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="CR.Rate" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="tbC02CRR" runat="server" Text='<%# Bind("C02CRR") %>' Style="border-color: #F0F0F0; text-align: center; border-radius: 5px;" Width="70" Height="18" onfocusin="JavaScript:gvViewTermOfCampaignOnFocusIn(this, 'tbC02CRR')" onfocusout="JavaScript:gvViewTermOfCampaignOnFocusOut(this, 'tbC02CRR')" OnChange="JavaScript:ValidateRateTermProductCRR(this); chkNum(this);" OnKeyPress="return chkNumber(this)" OnTextChanged="tbC02CRR_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="INF.Rate" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="tbC02IFR" runat="server" Text='<%# Bind("C02IFR") %>' Style="border-color: #F0F0F0; text-align: center; border-radius: 5px;" Width="70" Height="18" onfocusin="JavaScript:gvViewTermOfCampaignOnFocusIn(this, 'tbC02IFR')" onfocusout="JavaScript:gvViewTermOfCampaignOnFocusOut(this, 'tbC02IFR')" OnChange="JavaScript:ValidateRateTermProductINF(this); chkNum(this);" OnKeyPress="return chkNumberMinus(this)" OnTextChanged="tbC02IFR_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Rate(%)" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="tbRATES" runat="server" Text='<%# Bind("RATES") %>' Style="border-color: #F0F0F0; text-align: center; border-radius: 5px;" Width="70" Height="18" onfocusin="JavaScript:gvViewTermOfCampaignOnFocusIn(this, 'tbRATES')" onfocusout="JavaScript:gvViewTermOfCampaignOnFocusOut(this, 'tbRATES')" OnChange="JavaScript:ValidateRateTermProductRates(this); chkNum(this);" OnKeyPress="return chkNumberMinus(this)" OnTextChanged="tbRATES_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="C02AIR" HeaderText="Avg INT.Rate" SortExpression="C02AIR">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="C02ACR" HeaderText="Avg CR.Rate" SortExpression="C02ACR">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                    </Columns>
                                                                                    <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                    <AlternatingRowStyle BackColor="White" />
                                                                                    <FooterStyle BackColor="#CCCC99" />
                                                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />

                                                                                    <RowStyle BackColor="#F7F7DE" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="150">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="center" height="33px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7; width: 100px;">&nbsp;Sub Seq. list</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" height="24px" style="font-weight: 100; width: 80px;">
                                                                                        <div style="height: 150px; width: 150px; overflow-x: auto; overflow-y: auto; background-color: #F1F1F1; margin-top: 5px;">
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:Label ID="lblSubSeqList" runat="server"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td width="11"></td>
                                                                        <td>

                                                                            <table border="0" width="100%" cellpadding="5" cellspacing="0" style="table-layout: fixed; font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Panel runat="server" ID="controlSubdetail">
                                                                                            <table border="0" width="100%" cellpadding="0" cellspacing="3">
                                                                                                <tr>
                                                                                                    <td width="200" align="left">
                                                                                                        <asp:Label ID="Label54" runat="server" Text="Sub Seq.Detail" Width="120px"></asp:Label>
                                                                                                    </td>
                                                                                                    <td width="100%"></td>
                                                                                                    <td width="130" align="left">
                                                                                                        <asp:DropDownList ID="ddlSelectProduct" runat="server" Height="28" Width="130px" Style="margin-left: 100px;" AutoPostBack="True" Enabled="true" OnSelectedIndexChanged="ddlSelectProduct_onchange">
                                                                                                            <asp:ListItem Value="productType1" Selected="true">1 : All Product Item</asp:ListItem>
                                                                                                            <asp:ListItem Value="productType2">2 : All Product Item and Have Except Item (Code)</asp:ListItem>
                                                                                                            <asp:ListItem Value="productType3">3 : All Product Item and Have Except Item (Type)</asp:ListItem>
                                                                                                            <asp:ListItem Value="productType4">4 : Some Product Item</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="optionAddItem" runat="server" Height="28" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="optionAddItem_SelectedIndexChanged">
                                                                                                            <asp:ListItem Value="allSeq" Selected="True">All Sub Seq.</asp:ListItem>
                                                                                                            <asp:ListItem Value="onlySeq">Only Sub Seq.</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>

                                                                                                    <td width="120">
                                                                                                        <asp:DropDownList ID="ddlSelectItem" runat="server" Height="28" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlCampaignType_SelectedIndexChanged">
                                                                                                            <asp:ListItem Value="productType1" Selected="true">List Item Selected</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td width="50" align="left">
                                                                                                        <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" OnClick="SEARCH_ITEM" ID="serachProductCampaign" ValidationGroup="false">
                                                                                                            <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                                            <Border BorderStyle="None"></Border>
                                                                                                            <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                                            <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                                        </dx:ASPxButton>

                                                                                                    </td>
                                                                                                    <td width="120" align="right">
                                                                                                        <asp:Label Width="60" runat="server">Price :</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" Width="70" Height="22" ID="txtPriceMin" Text="" Enabled="true" placeholder="Price Min" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>

                                                                                                    </td>
                                                                                                    <td width="10">-
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox runat="server" Width="70" Height="22" ID="txtPriceMax" Text="" Enabled="true" placeholder="Price Max" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>

                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dx:ASPxButton ID="btnAddProductItem" runat="server" Enabled="true" Text="Add Item" ValidationGroup="false" Cursor="pointer" OnClick="ADD_PRODUCT_ITEM" AutoPostBack="False" Width="100px">
                                                                                                            <Image Url="~\Images\icon\addPlus.png"></Image>
                                                                                                            <DisabledStyle CssClass="imgGreyscal">
                                                                                                            </DisabledStyle>
                                                                                                        </dx:ASPxButton>
                                                                                                    </td>
                                                                                                    <td align="right">
                                                                                                        <dx:ASPxButton ID="btnClearProductItem" runat="server" Enabled="true" Text="Clear Item" ValidationGroup="false" CssClass="imgNormal3" Cursor="pointer" Width="100px" OnClick="CLEAR_PRODUCT_ITEM" AutoPostBack="False">
                                                                                                            <Image Url="~\Images\icon\deletenew.png" Height="16" Width="16"></Image>
                                                                                                            <DisabledStyle CssClass="imgGreyscal">
                                                                                                            </DisabledStyle>
                                                                                                        </dx:ASPxButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>

                                                                                    </td>

                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" width="100%" height="24px" style="font-weight: 100;">
                                                                                        <div style="height: 150px; width: auto; overflow-x: auto; overflow-y: auto; background-color: #F1F1F1; margin-top: 5px;">
                                                                                            <asp:GridView ID="gvViewTermOfCampaignDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                                EmptyDataText="No Records Found" ShowHeaderWhenEmpty="true" OnRowDataBound="CAMPAIGN_DETAIL_ROW_BOUND"
                                                                                                OnSelectedIndexChanging="gvViewTermOfCampaignDetailSelectedIndexChanging" OnRowDeleting="gvViewTermOfCampaignDetail_RowDeleting">
                                                                                                <Columns>
                                                                                                    <asp:CommandField ShowDeleteButton="true" HeaderText="#" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-Height="10px" ItemStyle-Width="10px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="10px" Width="10px"></ItemStyle>
                                                                                                    </asp:CommandField>
                                                                                                    <asp:BoundField DataField="SubSeq" HeaderText="Sub Seq." ReadOnly="True">
                                                                                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                                                                                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Type" HeaderText="Type">
                                                                                                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Code" HeaderText="Code">
                                                                                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Price" HeaderText="Price">
                                                                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:ButtonField CommandName="Select" HeaderText="Edit" ButtonType="Image" Visible="false"
                                                                                                        ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                        <HeaderStyle Width="50px" HorizontalAlign="Center"></HeaderStyle>
                                                                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                                                                    </asp:ButtonField>
                                                                                                </Columns>
                                                                                                <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
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
                                                                            </table>

                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </dx:ContentControl>
                                                        </ContentCollection>
                                                    </dx:TabPage>

                                                    <dx:TabPage Text="Share Sub." Name="ShareSub">
                                                        <TabStyle Font-Bold="True">
                                                        </TabStyle>
                                                        <ContentCollection>
                                                            <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                                <table cellpadding="5" cellspacing="0" width="1150px">
                                                                    <tr>
                                                                        <td width="430px" valign="top">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label31" runat="server" Text="For Application"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" valign="top" height="154px">
                                                                                        <div style="width: 100%; height: 100%; overflow-x: auto; overflow-y: auto; font-size: small; font-weight: 100;">
                                                                                            <asp:GridView ID="gvApplicationType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" DataKeyNames="GN61CD">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            Select
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox ID="cbSelect" runat="server" OnClick="return false;" />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            Code
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblApplicationTypeCode" runat="server" Text='<%#Eval("GN61CD") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle Width="30px" HorizontalAlign="Center" />
                                                                                                        <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="GN61DT" HeaderText="Name Thai"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="GN61DE" HeaderText="Name English"></asp:BoundField>
                                                                                                </Columns>
                                                                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
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
                                                                        <td width="150px" valign="top">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label32" runat="server" Width="90" Text="Base Rate"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" height="24px; font-weight: 100;">
                                                                                        <asp:TextBox runat="server" Width="70px" ID="txtBaseRate" Height="35px" Font-Size="X-Large" Style="text-align: center;" ReadOnly="true"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label36" runat="server" Text="Type Sub"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="font-size: small; font-weight: 100;">
                                                                                        <asp:RadioButtonList ID="rdoTypeSub" runat="server" AutoPostBack="true" OnClick="return false;" OnSelectedIndexChanged="rdoTypeSub_SelectedIndexChanged">
                                                                                            <asp:ListItem Text="Rate" Value="R" Selected="True"></asp:ListItem>
                                                                                            <asp:ListItem Text="Installment" Value="I"></asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td valign="top" width="100px" rowspan="2">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label33" runat="server" Text="Share Sub"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" valign="top" height="154px" style="font-size: small; font-weight: 100;">
                                                                                        <asp:CheckBoxList ID="ckbShareSub" runat="server" AutoPostBack="true" OnClick="return false;" OnSelectedIndexChanged="ckbShareSub_SelectedIndexChanged">
                                                                                            <asp:ListItem Text="Maker" Value="Maker"></asp:ListItem>
                                                                                            <asp:ListItem Text="Vendor" Value="Vendor"></asp:ListItem>
                                                                                            <asp:ListItem Text="ESB" Value="ESB"></asp:ListItem>
                                                                                        </asp:CheckBoxList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td valign="top" width="250px">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label35" runat="server" Text="Vendor Payment"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="font-weight: 100;">
                                                                                        <asp:RadioButtonList ID="rdoVendorPayment" runat="server">
                                                                                            <asp:ListItem Value="B" Text="Deduct at payment" Selected="True"></asp:ListItem>
                                                                                            <asp:ListItem Value="A" Text="Deduct after campaign closing"></asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <br />
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label37" runat="server" Text="Vendor Credit Days"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" valign="middle" height="49px" style="font-weight: 100">
                                                                                        <asp:TextBox runat="server" Width="80" ID="txtVendorCreditDays" Text="90" OnKeyPress="return chkNumberOnly(this)"></asp:TextBox>
                                                                                        &nbsp;Day(s)
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <div style="padding: 5px 5px 5px 5px;">
                                                                    <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                        <tr>
                                                                            <td align="center" valign="top" style="font-weight: 100;">
                                                                                <div style="width: 100%; height: 200px; border: 0em solid #808080; overflow: auto;">
                                                                                    <asp:GridView ID="gvPartyRate" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                        BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                                                                        OnPageIndexChanging="gvPartyRatePageIndexChanging">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="RATES" HeaderText="Rate" SortExpression="RATES">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="C05STO" HeaderText="Total Term" SortExpression="C05STO">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="C05CSQ" HeaderText="Sub Seq." SortExpression="C05CSQ">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="C05RSQ" HeaderText="Seq" SortExpression="C05RSQ">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="C05STM" HeaderText="Term" SortExpression="C05STM">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="C05SBT" HeaderText="Type" SortExpression="C05SBT">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Easybuy" HeaderText="Easybuy" SortExpression="Easybuy">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="ESubRate" HeaderText="E.Sub Rate" SortExpression="ESubRate">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Maker" HeaderText="Maker" SortExpression="Maker">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="MSubRate" HeaderText="M.Sub Rate" SortExpression="MSubRate">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="VSubRate" HeaderText="V.Sub Rate" SortExpression="VSubRate">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="CampaignStartTerm" HeaderText="CampaignStartTerm" SortExpression="CampaignStartTerm">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="CampaignEndTerm" HeaderText="CampaignEndTerm" SortExpression="CampaignEndTerm">
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                        <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                        <AlternatingRowStyle BackColor="White" />
                                                                                        <FooterStyle BackColor="#CCCC99" />
                                                                                        <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                                                        <RowStyle BackColor="#F7F7DE" />
                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </dx:ContentControl>
                                                        </ContentCollection>
                                                    </dx:TabPage>

                                                    <dx:TabPage Text="Vendor List" Name="VendorList">
                                                        <TabStyle Font-Bold="True">
                                                        </TabStyle>
                                                        <ContentCollection>
                                                            <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                                                                <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                    <tr>
                                                                        <td colspan="4" align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                            <asp:Label ID="Label38" runat="server" Text="Select Vendor"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 30px;">
                                                                        <td width="30px"></td>
                                                                        <td width="200px" valign="middle" align="left" height="30px" style="font-weight: 100;">
                                                                            <asp:RadioButtonList ID="rdbSelectVendor" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdbSelectVendor_SelectedIndexChanged">
                                                                                <asp:ListItem Text="All Vendor" Value="AV" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="Some Vendor" Value="SV"></asp:ListItem>

                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxButton ID="btnVendorListSearch" runat="server" Text="Search Vendor" Height="25px" Width="130px" OnClick="btnVendorListSearch_Click" ToolTip="Find Vendor" Visible="false" CausesValidation="false">
                                                                                <Image Url="~\Images\icon\search.png" Width="18px"></Image>
                                                                                <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                            </dx:ASPxButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnVendorListAdd" runat="server" Text="Add Vendor" Width="125px" EnableClientSideAPI="true" ClientEnabled="true" ClientInstanceName="btnVendorListAdd" OnClick="btnVendorListAdd_Click" OnClientClick="btnVendorListAddClick" ToolTip="Find Vendor" Visible="false" CausesValidation="false"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />

                                                                <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                    <tr>
                                                                        <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                            <asp:Label ID="Label43" runat="server" Text="Select Vendor"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="font-weight: 100;">
                                                                            <div style="height: 200px; overflow-x: auto; overflow-y: auto; background-color: #F1F1F1; margin-top: 5px;">
                                                                                <asp:GridView ID="gvVendorList" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                    BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" OnRowDataBound="GV_VENDOR_LIST_ROWDATABOUND" OnRowDeleting="gvVendorList_RowDeleting"
                                                                                    EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                    ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                                                                    OnPageIndexChanging="gvVendorListPageIndexChanging">
                                                                                    <Columns>
                                                                                        <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-Height="10px" ItemStyle-Width="10px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Height="10px" Width="10px"></ItemStyle>
                                                                                        </asp:CommandField>
                                                                                        <asp:BoundField DataField="C08VEN" HeaderText="Vendor Code">
                                                                                            <HeaderStyle Width="180px" HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="180px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="P10NAM" HeaderText="Vendor Name">
                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                            <ItemStyle HorizontalAlign="left" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="P16RNK" HeaderText="Rank">
                                                                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="P12ODR" HeaderText="% O/D">
                                                                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="P12WOR" HeaderText="% W/O">
                                                                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="P16END" HeaderText="Expire Vendor of Campaign">
                                                                                            <HeaderStyle Width="200px" HorizontalAlign="Center" />
                                                                                            <ItemStyle Width="200px" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                    </Columns>
                                                                                    <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                                    <AlternatingRowStyle BackColor="White" />
                                                                                    <FooterStyle BackColor="#CCCC99" />
                                                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                                                    <RowStyle BackColor="#F7F7DE" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </dx:ContentControl>
                                                        </ContentCollection>
                                                    </dx:TabPage>

                                                    <dx:TabPage Text="Branch | Note" Name="BranchNote">
                                                        <TabStyle Font-Bold="True">
                                                        </TabStyle>
                                                        <ContentCollection>
                                                            <dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="300px" height="80px" valign="top">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label44" runat="server" Text="Marketing Code"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%">
                                                                                            <tr valign="top">
                                                                                                <td>
                                                                                                    <asp:TextBox runat="server" Width="100%" ID="txtMarketingCode"></asp:TextBox>
                                                                                                </td>
                                                                                                <td width="20" align="right" style="padding-left: 10px">

                                                                                                    <dx:ASPxButton ID="btnSelectMarketing" runat="server" OnClick="SELECT_MARKETING_CODE" EnableDefaultAppearance="False" ValidationGroup="false" Cursor="pointer">
                                                                                                        <Image Url="~\Images\icon\search.png" Width="20"></Image>
                                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                                        <Border BorderStyle="None" />
                                                                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                                                                    </dx:ASPxButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td rowspan="2" valign="top" style="padding-left: 10px">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="table-layout: fixed; font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label45" runat="server" Text="List Note"></asp:Label>

                                                                                    </td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td rowspan="2">
                                                                                                    <div id="dataListSelected" style="width: 550px; height: 385px; margin-bottom: 8px; background-color: #FFFFB9; overflow-y: auto"></div>
                                                                                                </td>
                                                                                                <td height="40px" width="100%">
                                                                                                    <dx:ASPxMemo ID="txtNote" runat="server" Width="370" Height="350px"></dx:ASPxMemo>
                                                                                                </td>
                                                                                                <td rowspan="2" width="20%"></td>
                                                                                            </tr>
                                                                                            <tr>

                                                                                                <td align="left">
                                                                                                    <table cellpadding="5">
                                                                                                        <tr align="center">
                                                                                                            <td align="right" width="175">
                                                                                                                <dx:ASPxButton Enabled="false" ID="btnSaveNote" runat="server" Text="Save Note" Height="30" Width="110px" Cursor="pointer" OnClick="btnSaveNoteClick" ValidationGroup="false">
                                                                                                                    <Image Url="~\Images\icon\save.png" Width="16"></Image>
                                                                                                                    <DisabledStyle CssClass="imgGreyscal">
                                                                                                                    </DisabledStyle>
                                                                                                                </dx:ASPxButton>
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <dx:ASPxButton Enabled="false" ID="btnCancelNote" runat="server" Text="Clear Note" Height="30" Width="110px" Cursor="pointer" OnClick="btnCancelNoteClick">
                                                                                                                    <Image Url="~\Images\icon\close.png" Width="16"></Image>
                                                                                                                    <DisabledStyle CssClass="imgGreyscal">
                                                                                                                    </DisabledStyle>
                                                                                                                </dx:ASPxButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" valign="top" style="padding-top: 10px;">
                                                                            <table width="100%" cellpadding="5" cellspacing="0" style="font-family: Tahoma; font-size: small; font-weight: 700; border-radius: 10px; border: 2px solid #d2e2f7; background-color: #eff4f9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                                                <tr>
                                                                                    <td align="left" height="24px" style="border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                        <asp:Label ID="Label46" runat="server" Text="Branch"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="310px" align="center" valign="top" style="font-weight: 100;">
                                                                                        <div style="width: 100%; height: 314px; border: 0em solid #808080; overflow: auto;">
                                                                                            <asp:GridView ID="gvBranch" runat="server" AutoGenerateColumns="False" BackColor="White" OnRowDataBound="GV_BRANCH_ROWDATABOUND"
                                                                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found">
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
                                                                                                    <asp:BoundField DataField="T1BRN" HeaderText="Branch Code">
                                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="T1BNME" HeaderText="Branch Name">
                                                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                </Columns>
                                                                                                <EmptyDataRowStyle BackColor="#D4668D" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
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

                                                </TabPages>

                                            </dx:ASPxPageControl>
                                            <%--End Contain tab panel --%>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>

                            <dx1:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Height="150px" Width="250px">
                            </dx1:ASPxLoadingPanel>
                            <div style="position: fixed;">
                                <dx:ASPxPopupControl ID="popupChangePrice" ClientInstanceName="popupChangePrice"
                                    ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                    PopupVerticalAlign="Middle" HeaderText="Change Price"
                                    AllowDragging="True" Modal="True" Width="800px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                    CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css" CloseAction="CloseButton">
                                    <%--  <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>--%>
                                    <ContentStyle HorizontalAlign="center" VerticalAlign="Top">
                                    </ContentStyle>
                                    <HeaderStyle>
                                        <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                        <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                    </HeaderStyle>
                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="PopupControlContentControl23" runat="server" SupportsDisabledAttribute="True">

                                            <table width="100%" align="center">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="criteriaSubSeq" runat="server" Height="28" Width="120px">
                                                            <asp:ListItem Value="onlySeq" Selected="True">This Sub Seq.</asp:ListItem>
                                                            <asp:ListItem Value="allSeq">All Sub Seq.</asp:ListItem>

                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="criteriaProduct" runat="server" Height="28" Width="120px">

                                                            <asp:ListItem Value="thisProducts" Selected="True">This Product</asp:ListItem>
                                                            <asp:ListItem Value="allProducts">All Product</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="right" width="75">Min Price : 
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" Width="100" Height="22" ID="minPriceChange" Text="" Enabled="true" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                    </td>
                                                    <td align="right" width="75">Max Price : 
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox runat="server" Width="100" Height="22" ID="maxPriceChange" Text="" Enabled="true" OnChange="JavaScript:chkNum(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                    </td>
                                                    <td align="center" width="100">
                                                        <dx:ASPxButton ID="btnConfirmSavePrice" runat="server" value="modeSave" Text="SAVE" CssClass="imgNormal" Cursor="pointer" OnClick="CLICK_CONFIRM_SAVE_PRICE">
                                                            <Image Url="~\Images\icon\save.png" Width="16"></Image>

                                                            <ClientSideEvents Click="function(s, e) { popupChangePrice.Hide(); }"></ClientSideEvents>
                                                            <DisabledStyle CssClass="imgGreyscal">
                                                            </DisabledStyle>
                                                        </dx:ASPxButton>
                                                    </td>

                                                </tr>

                                            </table>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                            </div>
                            <dx:ASPxPopupControl ID="popupSearchCampaign" ClientInstanceName="popupSearchCampaign"
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
                                                <td align="center">You want to Load data from other campaign ?
                                              
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnLoadCampaign" runat="server" value="Yes" ValidationGroup="false" Text="Yes" Width="100px"
                                                        OnClick="BTN_LOAD_CAMPAIGN">
                                                        <ClientSideEvents Click="function(s, e) { popupSearchCampaign.Hide();}" />

                                                    </dx:ASPxButton>

                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnNotSave" runat="server" value="No" Text="No" Width="100px" OnClick="BTN_NO_LOAD_CAMPAIGN" ValidationGroup="false">

                                                        <ClientSideEvents Click="function(s, e) { popupSearchCampaign.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>

                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="popupConfirmSaveCampaign" ClientInstanceName="popupConfirmSaveCampaign"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl14" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <dx:ASPxLabel ID="lblConfirmSave" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #003D85;">
                                                    </dx:ASPxLabel>
                                                </td>

                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">Code :                            
                                              
                                                                <dx:ASPxLabel ID="lblCodeCampaign" runat="server" Font-Names="Tahoma" Font-Size="Small">
                                                                </dx:ASPxLabel>
                                                </td>

                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>

                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblNameCampaign" runat="server" Font-Names="Tahoma" Font-Size="Small">
                                                    </dx:ASPxLabel>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox Visible="false" ID="btnStatus" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right" width="50%">
                                                    <dx:ASPxButton ID="btnSaveData" runat="server" value="Yes" ValidationGroup="false" Text="Yes" Width="100px"
                                                        OnClick="BTN_CONFIRM_DATA_ALL">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmSaveCampaign.Hide();}" />

                                                    </dx:ASPxButton>

                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="noSaveData" runat="server" value="No" Text="No" Width="100px" ValidationGroup="false">

                                                        <ClientSideEvents Click="function(s, e) { popupConfirmSaveCampaign.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>

                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="popupConfirmUpdateCampaign" ClientInstanceName="popupConfirmUpdateCampaign"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText=""
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl19" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #003D85;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Update Campaign Status." Font-Names="Tahoma" Font-Size="Small"></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:RadioButtonList ID="rdoUpdateStatus" runat="server" RepeatDirection="Horizontal" CellPadding="3" CellSpacing="2">
                                                        <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="Reject" Value="E"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <%--<dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Tahoma" Font-Size="Small" ></dx:ASPxLabel>--%>  
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <dx:ASPxButton ID="btnConfirmUpdateOK" runat="server" value="Yes" ValidationGroup="false" Text="Submit" Width="100px" OnClick="btnConfirmUpdateOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmUpdateCampaign.Hide();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <%--<td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmUpdateCancel" runat="server" value="No" Text="No" Width="100px" ValidationGroup="false">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmUpdateCampaign.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="popupConfirmDeleteCampaign" ClientInstanceName="popupConfirmDeleteCampaign"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText=""
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl20" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #003D85;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Delete Campaign." Font-Names="Tahoma" Font-Size="Small"></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <%--<dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Tahoma" Font-Size="Small" ></dx:ASPxLabel>--%>  
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmDeleteOK" runat="server" value="Yes" ValidationGroup="false" Text="Yes" Width="100px" OnClick="btnConfirmDeleteOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmDeleteCampaign.Hide();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="btnConfirmDeleteCancel" runat="server" value="No" Text="No" Width="100px" ValidationGroup="false">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmDeleteCampaign.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="popupConfirmNote" ClientInstanceName="popupConfirmNote"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText=""
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl21" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/icon/question.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #003D85;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <dx:ASPxLabel ID="lblconfirmSaveNote" runat="server" Text="Confirm save note." Font-Names="Tahoma" Font-Size="Small"></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <%--<dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Names="Tahoma" Font-Size="Small" ></dx:ASPxLabel>--%>  
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="yesConfirm" runat="server" value="Yes" ValidationGroup="false" Text="Yes" Width="100px" OnClick="BTN_CONFIRM_SAVE_NOTE">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmNote.Hide();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left" width="50%">
                                                    <dx:ASPxButton ID="noConfirm" runat="server" value="No" Text="No" Width="100px" ValidationGroup="false">
                                                        <ClientSideEvents Click="function(s, e) { popupConfirmNote.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <div style="position: fixed;">
                                <dx:ASPxPopupControl ID="PopupAlertRate" ClientInstanceName="PopupAlertRate"
                                    ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                    PopupVerticalAlign="Middle" HeaderText="Alert"
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
                                                        <dx:ASPxButton ID="popupAlerRate" runat="server" Text="OK" Width="100px" ValidationGroup="false" OnClick="BTN_NO_LOAD_CAMPAIGN">

                                                            <ClientSideEvents Click="function(s, e) { PopupAlertRate.Hide(); }"></ClientSideEvents>
                                                        </dx:ASPxButton>
                                                    </td>

                                                </tr>

                                            </table>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                            </div>
                            <%--Popup Add  Maker--%>
                            <dx:ASPxPopupControl ID="PopupMsgError" ClientInstanceName="PopupMsgError" ShowPageScrollbarWhenModal="true"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server" SupportsDisabledAttribute="True">
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
                                                        <ClientSideEvents Click="function(s, e) { PopupMsgError.Hide(); }" />

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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl15" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/icon/success.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="lblMsgSuccess" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #1F6E42;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">Campaign :
                                                    <dx:ASPxLabel ID="lblMsgCmp" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #000000;">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="btnSuccess" runat="server" Text="OK" Width="100px" OnClick="BTN_RE_DATA_ALL">

                                                        <ClientSideEvents Click="function(s, e) { PopupMsgSuccess.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Add Maker --%>
                            <asp:HiddenField ID="hdfAddMaker" runat="server" />
                            <dx:ASPxPopupControl ID="PopupAddMakerCode" ClientInstanceName="PopupAddMakerCode" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Maker Code" AllowDragging="false" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                            <tr>
                                                <td align="left" width="90px" height="24px">
                                                    <asp:Label ID="lblPopupAddMakerCodeSelectMaker" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label29" runat="server" Style="text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td align="left" width="170px" height="24px">
                                                    <asp:DropDownList ID="ddlPoupAddMakerCodeSearchBy" Width="170px" runat="server">
                                                        <asp:ListItem Value="C" Selected="True">Code</asp:ListItem>
                                                        <asp:ListItem Value="D">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="150px" height="24px">
                                                    <asp:Label ID="lblPopupAddMakerCodeSearchBy" runat="server" Text="Select Maker"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label30" runat="server" Style="text-align: center;" Width="5px">:</asp:Label>
                                                </td>
                                                <td align="left" width="190px" height="24px">
                                                    <asp:TextBox ID="txtPopupAddMakerCodeSearchText" runat="server" Width="190px"></asp:TextBox>
                                                </td>
                                                <td align="left" width="200px" height="24px">
                                                    <asp:Button ID="btnPopupAddMakerCodeSearch" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_MAKER_SEARCH" ValidationGroup="false" />
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="btnPopupAddMakerCodeClear" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="CLICK_MAKER_CLEAR" ValidationGroup="false" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvMakerCode" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found"
                                                OnPageIndexChanging="gvMakerCodePageIndexChanging"
                                                OnSelectedIndexChanging="gvMakerCodeSelectedIndexChanging">
                                                <Columns>
                                                    <%--<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ControlStyle-Width="40px">
                                                        <ControlStyle Width="40px"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:ButtonField>--%>
                                                    <asp:BoundField DataField="T46MAK" HeaderText="Code" SortExpression="Code" ControlStyle-Width="50px">
                                                        <ControlStyle Width="50px"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T46TIC" HeaderText="Title" SortExpression="Title" ControlStyle-Width="50px">
                                                        <ControlStyle Width="50px"></ControlStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T46TNM" HeaderText="Maker Name" SortExpression="MakerName" />
                                                    <asp:BoundField DataField="T46ENM" HeaderText="Maker Name Eng." SortExpression="MakerNameEng" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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

                            <%-- Popup Add Vendor --%>
                            <asp:HiddenField ID="hdfAddVendor" runat="server" />
                            <dx:ASPxPopupControl ID="PopupAddVendor" ClientInstanceName="PopupAddVendor" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Vendor" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label102" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddVendorSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="VC" Selected="True">Vendor Code</asp:ListItem>
                                                        <asp:ListItem Value="VD">Vendor Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="lblPopupAddVendorSelectVendor" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search Vendor" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddVendorSearchText" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="btnPopupAddVendorSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorSearchClick" ValidationGroup="false" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnPopupAddVendorClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        OnClick="btnPopupAddVendorClearClick" ValidationGroup="false" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                AllowPaging="True" EmptyDataText="No records found" OnRowDataBound="GV_VENDOR_ROWDATABOUND"
                                                OnPageIndexChanging="gvVendorPageIndexChanging"
                                                OnSelectedIndexChanging="gvVendorSelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T00LTY" HeaderText="Loan Type" SortExpression="T00LTY"
                                                        Visible="false" />
                                                    <asp:BoundField DataField="T00TNM" HeaderText="LOan Desc." SortExpression="T00TNM"
                                                        Visible="false" />
                                                    <asp:BoundField DataField="P11VEN" HeaderText="Vendor" ReadOnly="True" SortExpression="P11VEN">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="p10nam" HeaderText="Description" SortExpression="p10nam" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                            <dx:ASPxPopupControl ID="PopupSearchItemCode" ClientInstanceName="PopupSearchItemCode" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Select product code" CloseAction="CloseButton" AllowDragging="false" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl16" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                            <tr>
                                                <td align="left" width="90px" height="24px">
                                                    <asp:Label ID="Label41" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label53" runat="server" Style="text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td align="left" width="190px" height="24px">
                                                    <asp:DropDownList ID="ddlItemCode" Width="170px" runat="server" Height="24">
                                                        <asp:ListItem Value="ICT" Selected="True">Type</asp:ListItem>
                                                        <asp:ListItem Value="ICC">Code</asp:ListItem>
                                                        <asp:ListItem Value="ICN">Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="150px" height="24px">
                                                    <asp:TextBox ID="txtProductCode" runat="server" Width="190px" Height="18"></asp:TextBox>
                                                    <%-- <asp:Label ID="Label54" runat="server" Text="Select Maker"></asp:Label>--%>
                                                </td>
                                                <%--    <td align="left" width="5px" height="24px">--%>
                                                <%-- <asp:Label ID="Label55" runat="server" Style="text-align: center;" Width="5px">:</asp:Label>--%>
                                                <%--  </td>--%>
                                                <td align="left" width="150px" height="24px"></td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="btnSearchCode" runat="server" Text="Search" Style="cursor: pointer;" Width="80px" Height="24px" ValidationGroup="false"
                                                        OnClick="BTN_SEARCH_PRODUCT_CODE" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnClearProductCode" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="BTN_CLEAR_PRODUCT_CODE" Style="cursor: pointer;" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: auto; height: 350px; overflow-x: auto; background-color: #F1F1F1; margin-top: 5px;">
                                            <asp:GridView ID="gv_searchItemCode" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                EmptyDataText="No records found">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-ForeColor="#000000">
                                                        <HeaderStyle HorizontalAlign="Center" Width="50" BackColor="#87CEFA"></HeaderStyle>
                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBoxSelectItemCode" runat="server" />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="T41TYP" HeaderText="Type" SortExpression="T41TYP" ControlStyle-Width="50px">
                                                        <ControlStyle Width="100px"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41COD" HeaderText="Code" SortExpression="T41COD" ControlStyle-Width="50px">
                                                        <ControlStyle Width="100px"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T41DES" HeaderText="Name" SortExpression="T41DES" />

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
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxButton ID="btnSelectItemCode" runat="server" Enabled="true" Text="Confirm Select" ValidationGroup="false" CssClass="imgNormal2" Cursor="pointer" OnClick="CONFIRM_SELECT_ITEM">
                                                        <Image Url="~\Images\icon\success.png" Width="18"></Image>
                                                        <%-- <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />--%>
                                                        <DisabledStyle CssClass="imgGreyscal">
                                                        </DisabledStyle>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="PopupSearchItemType" ClientInstanceName="PopupSearchItemType" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Select product Type" CloseAction="CloseButton" AllowDragging="false" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl17" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" style="font-family: Tahoma; font-size: small; font-weight: 700;">
                                            <tr>
                                                <td align="left" width="90px" height="24px">
                                                    <asp:Label ID="Label56" runat="server" Text="Search By"></asp:Label>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <asp:Label ID="Label57" runat="server" Style="text-align: center;" Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td align="left" width="190px" height="26px">
                                                    <asp:DropDownList ID="ddlItemType" Width="170px" runat="server" Height="24">
                                                        <asp:ListItem Value="ITT" Selected="True">Type</asp:ListItem>
                                                        <%-- <asp:ListItem Value="ITC">Code</asp:ListItem>--%>
                                                        <asp:ListItem Value="ITN">Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="150px" height="24px">
                                                    <%-- <asp:Label ID="Label58" runat="server" Text="Select"></asp:Label>--%>
                                                    <asp:TextBox ID="txtProductType" runat="server" Width="190px" Height="18"></asp:TextBox>
                                                </td>
                                                <td align="left" width="5px" height="24px">
                                                    <%-- <asp:Label ID="Label59" runat="server" Style="text-align: center;" Width="5px">:</asp:Label>--%>
                                                </td>
                                                <td align="left" width="140px" height="24px"></td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Button5" runat="server" Text="Search" Width="80px" Height="24px" Style="cursor: pointer;"
                                                        OnClick="BTN_SEARCH_PRODUCT_TYPE" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button6" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;"
                                                        OnClick="BTN_CLEAR_PRODUCT_TYPE" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: auto; height: 350px; overflow-x: auto; background-color: #F1F1F1; margin-top: 5px;">
                                            <asp:GridView ID="gv_searchItemType" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                EmptyDataText="No records found">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-ForeColor="#000000">
                                                        <HeaderStyle HorizontalAlign="Center" Width="50" BackColor="#87CEFA"></HeaderStyle>
                                                        <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBoxSelectItemType" runat="server" />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="T40TYP" HeaderText="Type" SortExpression="T40TYP" ControlStyle-Width="50px">
                                                        <ControlStyle Width="100px"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="t40fix" HeaderText="Code" SortExpression="t40fix" ControlStyle-Width="50px">
                                                        <ControlStyle Width="100px"></ControlStyle>
                                                        <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                        <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="t40des" HeaderText="Name" SortExpression="t40des" />

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
                                        <table width="100%">
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxButton ID="btnSelectItemType" runat="server" Enabled="true" Text="Confirm Select" ValidationGroup="false" CssClass="imgNormal2" Cursor="pointer" OnClick="CONFIRM_SELECT_ITEM">
                                                        <Image Url="~\Images\icon\success.png" Width="18"></Image>
                                                        <%-- <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />--%>
                                                        <DisabledStyle CssClass="imgGreyscal">
                                                        </DisabledStyle>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>


                            <%-- Popup Search Campaign --%>
                            <dx:ASPxPopupControl ID="popupSearchCampaignOld" ClientInstanceName="popupSearchCampaignOld"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Load Data from Other Campaign" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="680px" Height="600px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="680px" border="0" cellpadding="0" cellspacing="5" align="center">
                                            <tr>
                                                <td width="670px">
                                                    <div style="width: 660px; height: 580px; border: 0.05em solid #808080; overflow: auto; padding: 5px 5px 5px 5px">
                                                        <table width="640px" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                            <tr>
                                                                <td colspan="2" height="24px" align="left" style="font-weight: 700; border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                    <asp:Label ID="lblSearchCampaignOldHeader" runat="server" Text="Search Vendor"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="24px" align="left" width="250px">
                                                                    <asp:RadioButtonList ID="rdoSearchCampaignOld" runat="server" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Search Vendor" Value="SV" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Search Product" Value="SP"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <%--<td height="25px" align="left" width="320px">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxButton ID="btnSearchCampaign" runat="server" Text="Find Campaign" Height="25" Width="120px" Cursor="pointer" OnClick="btnSearchCampaign_Click">
                                                                                    <Image Url="~\Images\icon\search.png" Width="16"></Image>
                                                                                    <DisabledStyle CssClass="imgGreyscal">
                                                                                    </DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>&nbsp;</td>
                                                                            <td>
                                                                                <dx:ASPxButton ID="btnSearchCampaignClear" runat="server" Text="Clear" Height="25" Width="70px" Cursor="pointer" OnClick="btnSearchCampaignClear_Click">
                                                                                    <Image Url="~\Images\icon\refresh.png" Width="16"></Image>
                                                                                    <DisabledStyle CssClass="imgGreyscal">
                                                                                    </DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>--%>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <%-- Search form Campaign --%>
                                                        <table width="640px" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                            <tr>
                                                                <td rowspan="3" width="130px">
                                                                    <asp:CheckBox ID="ckbSearchFromCampaign" Text="From Campaign" runat="server" Width="120px" AutoPostBack="true" OnCheckedChanged="ckbSearchFromCampaign_CheckedChanged" />
                                                                </td>
                                                                <td width="110px">
                                                                    <asp:Label ID="lblSearchCampignCode" runat="server" Text="Campaign Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchCampaignCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchCampaignCode" runat="server" Height="18" OnClick="btnSearchCampaignCodeClick" AlternateText="ImageButton" ToolTip="Find Vendor" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" />
                                                                </td>
                                                                <td colspan="3" width="250px">
                                                                    <asp:Label ID="lblSearchCampaignName" runat="server" Text="Campaign Name"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchCampaignName" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5" width="440px">
                                                                    <table width="100%" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9;">
                                                                        <tr>
                                                                            <td colspan="3" height="24px" width="30%" align="left" style="font-weight: 700; border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                <asp:Label ID="lblSearchCampaignStatus" runat="server" Text="Campaign Status"></asp:Label>
                                                                            </td>
                                                                            <%--<%--</tr>--%>
                                                                            <%--<tr>--%>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rdoSearchCampaignStatus" runat="server" RepeatDirection="Horizontal" Width="70%" Enabled="false">
                                                                                    <asp:ListItem Value="ALL" Text="All"></asp:ListItem>
                                                                                    <asp:ListItem Value="NEW" Text="New" Selected="True"></asp:ListItem>
                                                                                    <asp:ListItem Value="APPROVE" Text="Approve"></asp:ListItem>
                                                                                    <asp:ListItem Value="END" Text="End"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="105px">
                                                                    <asp:Label ID="lblSearchStartDate" runat="server" Text="Start Date"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchStartDate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CCtxtSearchStartDate" runat="server" Format="dd/MM/eeee" PopupButtonID="btncalendarStart" TargetControlID="txtSearchStartDate" Enabled="false"></cc1:CalendarExtender>
                                                                </td>
                                                                <td width="15px">
                                                                    <br />
                                                                    <dx:ASPxButton ID="btncalendarStart" runat="server" EnableDefaultAppearance="False" Cursor="pointer" AutoPostBack="true" Enabled="false">
                                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                        <Border BorderStyle="None" />
                                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td width="105px">
                                                                    <asp:Label ID="lblSearchEndDate" runat="server" Text="End Date"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchEndDate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CCtxtSearchEndDate" runat="server" Format="dd/MM/eeee" PopupButtonID="btncalendarEnd" TargetControlID="txtSearchEndDate" Enabled="false"></cc1:CalendarExtender>
                                                                </td>
                                                                <td width="15px">
                                                                    <br />
                                                                    <dx:ASPxButton ID="btncalendarEnd" runat="server" EnableDefaultAppearance="False" Cursor="pointer" AutoPostBack="true" Enabled="false">
                                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                        <Border BorderStyle="None" />
                                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td width="115px">
                                                                    <asp:Label ID="lblSearchClosingLayBillDate" runat="server" Text="Closing Lay Bill Date"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchClosingLayBillDate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CCtxtSearchClosingLayBillDate" runat="server" Format="dd/MM/eeee" PopupButtonID="btncalendarClosing" TargetControlID="txtSearchClosingLayBillDate" Enabled="false"></cc1:CalendarExtender>
                                                                </td>
                                                                <td width="15px">
                                                                    <br />
                                                                    <dx:ASPxButton ID="btncalendarClosing" runat="server" EnableDefaultAppearance="False" Cursor="pointer" AutoPostBack="true" Enabled="false">
                                                                        <Image Url="~/ManageData/Images/CalendarBtn.gif"></Image>
                                                                        <Border BorderStyle="None" />
                                                                        <DisabledStyle CssClass="calendars"></DisabledStyle>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <%-- Search from Product --%>
                                                        <table width="640px" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                            <%-- Product Type --%>
                                                            <tr>
                                                                <td rowspan="5" width="130px">
                                                                    <asp:CheckBox ID="ckbSearchFromProduct" Text="From Product" runat="server" Width="120px" AutoPostBack="true" OnCheckedChanged="ckbSearchFromProduct_CheckedChanged" />
                                                                </td>
                                                                <td width="110px">
                                                                    <asp:Label ID="lblSearchProductTypeCode" runat="server" Text="Product Type"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductTypeCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchProductType" runat="server" OnClick="btnSearchProductTypeClick" Height="18" AlternateText="ImageButton" ToolTip="Find Campaign" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSearchProductTypeName" runat="server" Text="Type Name"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductTypeName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- Product Code --%>
                                                            <tr>
                                                                <td width="110px">
                                                                    <asp:Label ID="lblSearchProductCode" runat="server" Text="Product Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchProductCode" runat="server" Height="18" OnClick="btnSearchProductCodeClick" AlternateText="ImageButton" ToolTip="Find Campaign" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSearchProductName" runat="server" Text="Product Name"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- Product Brand --%>
                                                            <tr>
                                                                <td width="110px">
                                                                    <asp:Label ID="Label68" runat="server" Text="Brand Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductBarandCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchProductBrand" runat="server" Height="18" OnClick="btnSearchProductBrandClick" AlternateText="ImageButton" ToolTip="Find Campaign" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label74" runat="server" Text="Brand Name"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductBrandName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- Product Model --%>
                                                            <tr>
                                                                <td width="110px">
                                                                    <asp:Label ID="Label75" runat="server" Text="Model Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductModelCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchProductModel" runat="server" Height="18" OnClick="btnSearchProductModelClick" AlternateText="ImageButton" ToolTip="Find Campaign" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label76" runat="server" Text="Model Name"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductModelName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- Product Item --%>
                                                            <tr>
                                                                <td width="110px">
                                                                    <asp:Label ID="Label77" runat="server" Text="Product Item"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductItemCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchProductItem" runat="server" Height="18" OnClick="btnSearchProductItemClick" AlternateText="ImageButton" ToolTip="Find Campaign" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label78" runat="server" Text="Product Item Description"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchProductItemDescription" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <%-- Search from Vendor --%>
                                                        <table width="640px" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9; box-shadow: 0px 0px 5px #aaaaaa;">
                                                            <tr>
                                                                <td rowspan="3" width="130px">
                                                                    <asp:CheckBox ID="ckbSearchFromVendor" Text="From Vendor" runat="server" Width="120px" AutoPostBack="true" OnCheckedChanged="ckbSearchFromVendor_CheckedChanged" />
                                                                </td>
                                                                <td width="110px">
                                                                    <asp:Label ID="Label79" runat="server" Text="Vendor Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearhVendorCode" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td width="15px">
                                                                    <br />
                                                                    <asp:ImageButton ID="btnSearchVendor" runat="server" Height="18" OnClick="btnSearchVendorClick" AlternateText="ImageButton" ToolTip="Find Vendor" ImageUrl="~\Images\icon\search.png" ImageAlign="AbsMiddle" Enabled="false" CausesValidation="false" />
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:Label ID="Label80" runat="server" Text="Vendor Name"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearchVendorName" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label14" runat="server" Text="Old Vendor Code"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearhVendorCodeOld" runat="server" Width="100" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td colspan="4">
                                                                    <table width="90%" cellspacing="0" cellpadding="5" style="font-family: Tahoma; font-size: small; border-radius: 10px; border: 2px solid #D2E2F7; background-color: #EFF4F9;">
                                                                        <tr>
                                                                            <td colspan="1" width="25%" height="24px" align="left" style="font-weight: 700; border-radius: 8px 8px 0px 0px; border: 2px solid #D2E2F7; background-color: #D2E2F7;">
                                                                                <asp:Label ID="Label82" runat="server" Text="Campaign Status"></asp:Label>
                                                                            </td>
                                                                            <%--</tr>--%>
                                                                            <%--<tr>--%>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rdoSearchFromVendorCampaignStatus" runat="server" RepeatDirection="Horizontal" Width="75%" Enabled="false">
                                                                                    <asp:ListItem Value="ALL" Text="All"></asp:ListItem>
                                                                                    <asp:ListItem Value="NEW" Text="New" Selected="True"></asp:ListItem>
                                                                                    <asp:ListItem Value="APPROVE" Text="Approve"></asp:ListItem>
                                                                                    <asp:ListItem Value="END" Text="End"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label25" runat="server" Text="Rank"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearhVendorRank" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:Label ID="Label26" runat="server" Text="Start Date Rank"></asp:Label>
                                                                    <%--&nbsp;&nbsp;
                                                                    <asp:Label ID="Label39" runat="server" Text="End Date Rank"></asp:Label>--%>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearhVendorStartDateRank" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                    <%--&nbsp;
                                                                    <asp:TextBox ID="TextBox1" runat="server" Width="100px" Enabled="false"></asp:TextBox>--%>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label28" runat="server" Text="End Date Rank"></asp:Label>
                                                                    <br />
                                                                    <asp:TextBox ID="txtSearhVendorEndDateRank" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>


                            <%--Start iframe load campaign --%>
                            <dx:ASPxPopupControl ID="popupLoadCampaign" ClientInstanceName="popupLoadCampaign"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Load Data from Other Campaign" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="1285px" Height="600px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <iframe id="myiframeLoadCampaign" name="myiframe" src="CampaignLoadForm.aspx" style="margin-top: -10px; overflow-y: auto;"
                                            width="100%" height="650" frameborder="0"></iframe>
                                    </dx:PopupControlContentControl>

                                </ContentCollection>
                                <ClientSideEvents CloseUp="function(s, e) { __doPostBack('btnSelectCampaign', ''); }" />
                            </dx:ASPxPopupControl>
                            <%--end iframe load campaign --%>

                            <%--Start iframe Search Vendor--%>
                            <dx:ASPxPopupControl ID="popupSearchVendor" ClientInstanceName="popupSearchVendor" EnableClientSideAPI="true"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Search Vendor" CloseAction="CloseButton" OnWindowCallback="popupSearchVendor_WindowCallback"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="1285px" Height="600px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="100%">
                                            <tr valign="top">
                                                <td width="60%" align="center">

                                                    <div style="box-shadow: 2px 2px #AAAAAA; width: 100%; height: 650px; background-color: #F1F1F1; margin-top: 15px;">
                                                        <div>
                                                        </div>
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gv_listVendor_2" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No records found" AutoGenerateColumns="false" Width="100%"
                                                                        AllowPaging="true" PageSize="20" CellPadding="4" OnPageIndexChanging="gv_listVendor_PageIndexChanging_2">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <HeaderStyle HorizontalAlign="Center" Width="50" BackColor="#666666"></HeaderStyle>
                                                                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="CheckBoxSelectVendor_2" runat="server" OnCheckedChanged="CheckBoxSelectVendor_CheckedChanged" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="P10VEN" HeaderText="Vendor Code" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <HeaderStyle HorizontalAlign="Center" Width="130"></HeaderStyle>
                                                                                <ItemStyle Width="130px"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="P10NAM" HeaderText="Vendor Name" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemStyle></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle BackColor="#A8A8A8" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                        <SelectedRowStyle BorderStyle="Groove" BorderColor="#666666" ForeColor="#000A0F" BorderWidth="2" Font-Bold="True" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                                        <PagerStyle HorizontalAlign="Right" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td width="40%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="txtTitleCampaign_2" runat="server"></asp:Label><asp:Label ID="lblCopyCampaign_2" runat="server" Style="margin-right: 4px;"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <fieldset style="border-radius: 5px;">
                                                                    <legend>Vendor Filter</legend>
                                                                    <asp:Panel runat="server" ID="box_branch_filter_2">
                                                                        <table width="100%">
                                                                            <tr valign="middle">
                                                                                <td align="center" width="150px">

                                                                                    <dx:ASPxButton ID="btnMainSearch_2" OnClick="CLICK_MAIN_SEARCH_2" runat="server" Text="Find Vendor" Height="25" Width="120" CssClass="imgNormal2" Cursor="pointer">
                                                                                        <Image Url="~\Images\icon\find.png" Width="16"></Image>
                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                                        </DisabledStyle>
                                                                                    </dx:ASPxButton>
                                                                                </td>
                                                                                <td align="center" width="150px">

                                                                                    <dx:ASPxButton ID="btnMainDetail_2" ClientInstanceName="btnMainDetail_2" ClientEnabled="true" OnClick="CLICK_MAIN_DETAIL_2" runat="server" Text="Select Vendor" Height="25" Width="120" CssClass="imgNormal2" Cursor="pointer">
                                                                                        <Image Url="~\Images\icon\insert.png" Width="16"></Image>
                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                                        </DisabledStyle>
                                                                                    </dx:ASPxButton>
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel runat="server" ID="box_from_campaign_2">
                                                        <table width="98%" border="0" style="margin-left: 5px; margin-top: 8px; border-radius: 5px; border: 1px solid #6C6C6C; background-color: #eff4f9;">
                                                            <tr>
                                                                <td>
                                                                    <table width="98%" border="0">
                                                                        <tr>
                                                                            <td rowspan="5" width="120px">
                                                                                <asp:CheckBoxList ID="chbCampaign_2" Width="120" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_CHB_CAMPAIGN_2">
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
                                                                                <asp:TextBox runat="server" Height="20px" Width="90" ID="campaignID_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_CAMPAIGN_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="20">
                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="serachCampaign_2" OnClick="CLICK_SEARCH_CAMPAIGN_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>
                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td></td>
                                                                            <td colspan="5">
                                                                                <asp:TextBox runat="server" Height="20px" Width="259" ID="campaingName_2"></asp:TextBox>
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
                                                                                                <asp:RadioButtonList ID="campaignStatusCampaign_2" runat="server" RepeatDirection="Horizontal"
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
                                                                                <asp:TextBox ID="startDate_2" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtender1_2" runat="server" Enabled="True"
                                                                                    Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate_2">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="calendarStart_2" runat="server" ImageAlign="Bottom"
                                                                                    ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                            </td>
                                                                            <td></td>
                                                                            <td width="70" align="left">
                                                                                <asp:TextBox ID="endDate_2" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtender2_2" runat="server" Enabled="True"
                                                                                    Format="dd/MM/eeee" PopupButtonID="calendarEnd" TargetControlID="endDate_2">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                            <td width="20">
                                                                                <asp:ImageButton ID="calendarEnd_2" runat="server" ImageAlign="Bottom"
                                                                                    ImageUrl="~/ManageData/Images/CalendarBtn.gif" Width="23px" />
                                                                            </td>
                                                                            <td width="20px"></td>
                                                                            <td align="left" width="70px">
                                                                                <asp:TextBox ID="closingDate_2" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtender3_2" runat="server" Enabled="True"
                                                                                    Format="dd/MM/eeee" PopupButtonID="calendarClosing" TargetControlID="closingDate_2">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="calendarClosing_2" runat="server" ImageAlign="Bottom"
                                                                                    ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="box_from_product_2">
                                                        <table width="98%" border="0" style="margin-left: 5px; margin-top: 8px; border-radius: 5px; border: 1px solid #6C6C6C; background-color: #eff4f9;">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" border="0">
                                                                        <tr>
                                                                            <td rowspan="10" width="120px">
                                                                                <asp:CheckBoxList ID="chbProduct_2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_CHB_PRODUCT_2">
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
                                                                                <asp:TextBox runat="server" Width="90" Height="20px" ID="productType_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_PRODUCT_TYPE_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="50" align="left">

                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="serachProductType_2" OnClick="CLICK_SELECT_PRODUCT_TYPE_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="260px" ID="typeName_2"></asp:TextBox>

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
                                                                                <asp:TextBox runat="server" Width="90" Height="20px" ID="productCode_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_PRODUCT_CODE_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="50" align="left">
                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchProductCode_2" OnClick="CLICK_SELECT_PRODUCT_CODE_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>
                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="260px" ID="codeName_2"></asp:TextBox>

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
                                                                                <asp:TextBox runat="server" Height="20px" Width="90" ID="brandTxt_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_BRAND_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="50" align="left">
                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchBranchTxt_2" OnClick="CLICK_SELECT_BRAND_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="260px" ID="brandName_2"></asp:TextBox>
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
                                                                                <asp:TextBox runat="server" Width="90" Height="20px" ID="modelTxt_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_MODEL_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="50" align="left">

                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchModel_2" OnClick="CLICK_SELECT_MODEL_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />

                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="260px" ID="modelName_2"></asp:TextBox>

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
                                                                                <asp:TextBox runat="server" Height="20px" Width="90" ID="productItem_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_PRODUCT_ITEM_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="50" align="left">
                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchProductItem_2" OnClick="CLICK_SELECT_PRODUCT_ITEM_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="260px" ID="productItemDes_2"></asp:TextBox>
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" ID="box_from_vendor_2">
                                                        <table width="98%" border="0" style="margin-left: 5px; margin-top: 8px; border-radius: 5px; border: 1px solid #6C6C6C; background-color: #eff4f9;">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" border="0" style="table-layout: fixed;">

                                                                        <tr>
                                                                            <td rowspan="6" width="120px">
                                                                                <asp:CheckBoxList ID="chbVendor_2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ONCHANGE_CHB_VENDOR_2">
                                                                                    <asp:ListItem Value="1">From Vendor</asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                            <td width="98" style="height: 20px">Vendor Code</td>
                                                                            <td width="50" style="height: 20px"></td>

                                                                            <td style="height: 20px; width: 120px;">Vendor Name
                                                                            </td>
                                                                            <td width="30"></td>
                                                                            <td width="60" style="height: 20px">% O/D 
                                                                            </td>
                                                                            <td width="70" style="height: 20px">% W/O</td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="90" ID="vendorCode_2" OnKeyPress="return chkNumber(this)" OnTextChanged="TEXT_CHANGE_VENDOR_2" AutoPostBack="true"></asp:TextBox>
                                                                            </td>
                                                                            <td width="50">
                                                                                <dx:ASPxButton runat="server" Cursor="pointer" EnableDefaultAppearance="False" ID="searchVendor_2" OnClick="CLICK_SEARCH_VENDOR_2">
                                                                                    <Image Width="20px" Url="~\Images\icon\search.png"></Image>

                                                                                    <Border BorderStyle="None"></Border>
                                                                                    <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                    <DisabledStyle CssClass="imgGreyscal2"></DisabledStyle>
                                                                                </dx:ASPxButton>
                                                                            </td>

                                                                            <td style="width: 120px">
                                                                                <asp:TextBox runat="server" Height="20px" Width="110px" ID="vendorName_2"></asp:TextBox>
                                                                            </td>
                                                                            <td></td>
                                                                            <td width="38">
                                                                                <asp:TextBox runat="server" Height="20px" Width="50px" ID="odTxt_2" OnChange="JavaScript:chkNum3(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                            </td>

                                                                            <td width="38">
                                                                                <asp:TextBox runat="server" Height="20px" Width="50px" ID="wqTxt_2" OnChange="JavaScript:chkNum3(this)" OnKeyPress="return chkNumber(this)"></asp:TextBox>
                                                                            </td>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Old Vendor Code
                                                                            </td>
                                                                            <td></td>
                                                                            <td colspan="2">Rank
                                                                            </td>
                                                                            <td colspan="3"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="90" ID="oldVendorCode_2"></asp:TextBox>
                                                                            </td>
                                                                            <td></td>
                                                                            <td colspan="2">
                                                                                <asp:DropDownList ID="ddlRank_2" runat="server" Height="26" Width="118px" AppendDataBoundItems="true">
                                                                                </asp:DropDownList></td>
                                                                            <td></td>
                                                                            <td></td>
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
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <%--End iframe Search Vendor--%>

                            <%--Start iframe Search Product--%>
                            <dx:ASPxPopupControl ID="popupSearchProduct" ClientInstanceName="popupSearchProduct" EnableClientSideAPI="true"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Search Product" CloseAction="CloseButton" OnWindowCallback="popupSearchVendor_WindowCallback"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="1285px" Height="600px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl18" runat="server" SupportsDisabledAttribute="True">

                                        <table width="100%">
                                            <tr valign="top">
                                                <td width="50%" align="left">

                                                    <div style="width: 100%; height: 400px; background-color: #EFF4F9; margin-top: 0px;">

                                                        <table width="100%" border="0" style="height: 400px;">
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    <asp:GridView ID="gv_listVendor" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No records found" AutoGenerateColumns="false" Width="100%"
                                                                        AllowPaging="true" PageSize="15" OnPageIndexChanging="gv_listVendor_PageIndexChanging">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <HeaderStyle HorizontalAlign="Center" Width="50" BackColor="#666666"></HeaderStyle>
                                                                                <ItemStyle Width="10px" HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="CheckBoxSelectVendor" runat="server" OnCheckedChanged="CheckBoxSelectVendor_CheckedChanged" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="T42BRD" HeaderText="Product Code" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                                                <ItemStyle Width="100px"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="T42DES" HeaderText="Product Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <ItemStyle></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle BackColor="#DEDEDE" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                                        <SelectedRowStyle BorderStyle="Groove" BorderColor="#666666" ForeColor="#000A0F" BorderWidth="2" Font-Bold="True" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>

                                                </td>

                                                <td width="40%" rowspan="2">

                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <fieldset style="border-radius: 5px;">
                                                                    <legend>Product Filter</legend>
                                                                    <asp:Panel runat="server" ID="box_branch_filter">
                                                                        <table width="100%">
                                                                            <tr valign="middle">
                                                                                <td align="center" width="150px">

                                                                                    <dx:ASPxButton ID="btnMainSearch" OnClick="CLICK_MAIN_SEARCH" runat="server" Text="Find Product" Width="130" Height="25" CssClass="imgNormal4" Cursor="pointer">
                                                                                        <Image Url="~\Images\icon\find.png" Width="16"></Image>
                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                                        </DisabledStyle>
                                                                                    </dx:ASPxButton>
                                                                                </td>
                                                                                <td align="center" width="150px">

                                                                                    <dx:ASPxButton ID="btnMainDetail" ClientInstanceName="btnMainDetail" Width="130" ClientEnabled="true" OnClick="CLICK_MAIN_DETAIL" runat="server" Text="Select Product" Height="25" CssClass="imgNormal4" Cursor="pointer">
                                                                                        <Image Url="~\Images\icon\insert.png" Width="16"></Image>
                                                                                        <ClientSideEvents Click="function(s, e) { LoadingPanel.Show();}" />
                                                                                        <DisabledStyle CssClass="imgGreyscal">
                                                                                        </DisabledStyle>
                                                                                    </dx:ASPxButton>
                                                                                </td>
                                                                                <td></td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </fieldset>
                                                            </td>
                                                        </tr>

                                                    </table>
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
                                                                                <asp:TextBox ID="startDate_1" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtender1_1" runat="server" Enabled="True"
                                                                                    Format="dd/MM/eeee" PopupButtonID="calendarStart" TargetControlID="startDate_1">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="calendarStart_1" runat="server" ImageAlign="Bottom"
                                                                                    ImageUrl="~/ManageData/Images/CalendarBtn.gif" />
                                                                            </td>
                                                                            <td></td>
                                                                            <td width="70" align="left">
                                                                                <asp:TextBox ID="endDate_1" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtender2_1" runat="server" Enabled="True"
                                                                                    Format="dd/MM/eeee" PopupButtonID="calendarEnd" TargetControlID="endDate_1">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                            <td width="20">
                                                                                <asp:ImageButton ID="calendarEnd_1" runat="server" ImageAlign="Bottom"
                                                                                    ImageUrl="~/ManageData/Images/CalendarBtn.gif" Width="23px" />
                                                                            </td>
                                                                            <td width="20px"></td>
                                                                            <td align="left" width="70px">
                                                                                <asp:TextBox ID="closingDate_1" runat="server" Width="92px" Height="20px"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtender3_1" runat="server" Enabled="True"
                                                                                    Format="dd/MM/eeee" PopupButtonID="calendarClosing" TargetControlID="closingDate_1">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="calendarClosing_1" runat="server" ImageAlign="Bottom"
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

                                                                            <td style="height: 20px; width: 110px;">Vendor Name
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

                                                                            <td style="width: 110px">
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
                                                                            <td></td>
                                                                            <td colspan="2">Rank
                                                                            </td>
                                                                            <td colspan="3"></td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox runat="server" Height="20px" Width="90" ID="oldVendorCode"></asp:TextBox>
                                                                            </td>


                                                                            <td></td>

                                                                            <td colspan="2">
                                                                                <asp:DropDownList ID="ddlRank" runat="server" Height="26" Width="118px" AppendDataBoundItems="true">
                                                                                </asp:DropDownList></td>
                                                                            <td></td>
                                                                            <td></td>
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

                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table style="margin-top: 1px;">
                                                        <tr align="left">
                                                            <td>
                                                                <asp:Label ID="Label69" runat="server" Text=" Selected product list" Style="font-weight: bold; font-size: 14px; margin-top: 3px;"></asp:Label>

                                                            </td>
                                                        </tr>


                                                    </table>
                                                    <div style="width: 100%; height: 200px; max-height: 250px; background-color: #FEFCC8; margin-top: 0px;">

                                                        <table width="100%" style="height: 200px;">
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    <asp:GridView ID="gv_listSelected" runat="server" ShowHeaderWhenEmpty="true" EmptyDataText="No records found" AutoGenerateColumns="false" Width="100%"
                                                                        AllowPaging="true" PageSize="8" OnPageIndexChanging="gv_listSelected_PageIndexChanging" OnRowDeleting="gv_listSelected_RowDeleting">
                                                                        <AlternatingRowStyle BackColor="#FEFCC8" />
                                                                        <Columns>

                                                                            <asp:CommandField ShowDeleteButton="true" HeaderText="#" ButtonType="Image" DeleteImageUrl="~\Images\icon\click_trash.png" ItemStyle-Height="10px" ItemStyle-Width="10px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></ItemStyle>
                                                                            </asp:CommandField>
                                                                            <asp:BoundField DataField="T42BRD" HeaderText="Product Code" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <HeaderStyle HorizontalAlign="Center" Width="100"></HeaderStyle>
                                                                                <ItemStyle Width="100px"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="T42DES" HeaderText="Product Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="#FFFFFF">
                                                                                <ItemStyle></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <EmptyDataRowStyle BackColor="#DEDEDE" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />

                                                                        <RowStyle BackColor="#C9FECC" ForeColor="#333333" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:PopupControlContentControl>

                                </ContentCollection>
                                <ClientSideEvents CloseUp="function(s, e) { __doPostBack('btnSelectProduct', ''); }" />
                            </dx:ASPxPopupControl>
                            <%--End iframe Search Product--%>

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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl27" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image12" runat="server" ImageUrl="~/Images/icon/alert.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="ASPxButton4" runat="server" Text="OK" Width="100px">

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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl28" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/Images/icon/question.png" />

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
                                                    <dx:ASPxButton ID="ASPxButton5" runat="server" Text="OK" Width="100px">
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl33" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image14" runat="server" ImageUrl="~/Images/icon/alertRed.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton6" runat="server" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }" />

                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <dx1:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl34" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label124" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label125" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label126" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label127" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchCampaign" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchCampaignPopup" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_CAMPAIGN_POPUP" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearCampaignPopup" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="CLEAR_POPUP_CAMPAIGN" />
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl35" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label128" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label129" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label130" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label131" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProducttype" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchProductType" Style="cursor: pointer;" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_TYPE_POPUP" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearProductType" Style="cursor: pointer;" runat="server" Text="Clear" Width="80px" Height="24px" OnClick="CLEAR_POPUP_PRODUCTTYPE" />
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl36" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label132" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label133" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label134" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label135" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="TextBox1" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchProducCode" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_CODE"
                                                        Style="cursor: pointer;" />&nbsp;&nbsp;
                                                    <asp:Button ID="ClearProducCode" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;" OnClick="CLEAR_POPUP_PRODUCTCODE" />
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl37" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label136" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label137" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label138" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label139" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchBrands" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchingBrand" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_BRAND_POPUP"
                                                        Style="cursor: pointer;" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearBrand" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;" OnClick="CLEAR_POPUP_BRAND" />
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl38" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label140" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label141" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label142" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label143" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchModel" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchingModel" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_MODEL_POPUP"
                                                        Style="cursor: pointer;" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearingModel" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;" OnClick="CLEAR_POPUP_MODEL" />
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl39" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label144" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label145" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label146" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label147" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProductItem" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchingProductItem" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_ITEM_POPUP"
                                                        Style="cursor: pointer;" />&nbsp;&nbsp;
                                                    <asp:Button ID="clearingProductItem" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;" OnClick="CLEAR_POPUP_PRODUCT_ITEM" />
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
                            <dx:ASPxPopupControl ID="Popup_Search_Product" ClientInstanceName="Popup_Search_Product"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl40" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label148" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label149" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
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
                                                    <asp:Label ID="Label150" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchVendor" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Popup_searchVendor" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_VENDOR_POPUP"
                                                        Style="cursor: pointer;" />&nbsp;&nbsp;
                                                    <asp:Button ID="Popup_clearVendor" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;" OnClick="CLEAR_POPUP_VENDOR" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gvVendor_1" runat="server" AutoGenerateColumns="False" BackColor="White"
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


                            <%-- Popup Product Type --%>
                            <dx:ASPxPopupControl ID="PopupAddProductType" ClientInstanceName="PopupsAddProductType"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Type" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label83" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label84" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductTypeSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PTC" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PTD">Product Type Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 140px; height: 24px;">
                                                    <asp:Label ID="Label85" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Select Product Type" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label86" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductTypeSearchText" runat="server" Width="180px"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="btnPopupAddProductTypeSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductTypeSearchClick" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnPopupAddProductTypeClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductTypeClearClick" />
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
                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T40DES" HeaderText="Product Type Description" SortExpression="T40DES" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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

                            <%-- Popup Product Code --%>
                            <dx:ASPxPopupControl ID="PopupAddProductCode" ClientInstanceName="PopupAddProductCode"
                                ShowPageScrollbarWhenModal="true" runat="server" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" HeaderText="Product Code" CloseAction="CloseButton"
                                AllowDragging="True" AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px"
                                CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass"
                                SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label87" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label88" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductCodeSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PT" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PC">Product Code</asp:ListItem>
                                                        <asp:ListItem Value="PD">Product Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label89" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Select Product Type" Width="130px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label90" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductCodeSearchText" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="btnPopupAddProductCodeSearch" runat="server" Text="Search" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductCodeSearchClick" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnPopupAddProductCodeClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductCodeClearClick" />
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
                                                        <ItemStyle Width="100px" />
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
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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

                            <%-- Popup Product Brand --%>
                            <dx:ASPxPopupControl ID="PopupAddProductBrand" ClientInstanceName="PopupAddProductBrand" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Product Brand" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="850px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
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
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label91" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label92" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 160px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductBrandSearchBy" runat="server" Width="140px">
                                                        <asp:ListItem Value="PBC" Selected="True">Product Brand Code</asp:ListItem>
                                                        <asp:ListItem Value="PBN">Product Brand Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; height: 24px;">
                                                    <asp:Label ID="Label93" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Select Brand" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label94" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductBrandSearchText" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <asp:Button ID="btnPopupAddProductBrandSearch" runat="server" Text="Search" Width="80px"
                                                        Height="24px" BackColor="#66CCFF" OnClick="btnPopupAddProductBrandSearchClick" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnPopupAddProductBrandClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductBrandClearClick" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvProductBrand" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="gvProductBrandPageIndexChanging"
                                                OnSelectedIndexChanging="gvProductBrandSelectedIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="T42BRD" HeaderText="Product Brand Code" ReadOnly="True" SortExpression="T42BRD">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T42DES" HeaderText="Product Brand Name" SortExpression="T42DES" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                            <dx:ASPxPopupControl ID="PopupMarketCode" ClientInstanceName="PopupMarketCode" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Marketing Code" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="850px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label47" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label48" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 160px; height: 24px;">
                                                    <asp:DropDownList ID="ddl_SearchBy" runat="server" Width="140px">
                                                        <asp:ListItem Value="DMC" Selected="True"> Marketing Code </asp:ListItem>
                                                        <asp:ListItem Value="TMC"> Marketing Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 120px; height: 24px;">
                                                    <asp:Label ID="Label49" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Select By" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label50" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:TextBox ID="txt_Marketing" runat="server" Width="180px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <asp:Button ID="Button1" runat="server" Text="Search" Width="80px" Style="cursor: pointer;"
                                                        Height="24px" OnClick="BTN_SEARCH_MARKETING" ValidationGroup="False" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button2" runat="server" Text="Clear" Width="80px" Height="24px" Style="cursor: pointer;"
                                                        OnClick="BTN_SEARCH_CLEAR_MARKETING" ValidationGroup="False" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:GridView ID="gvMarketingCode" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="GV_PAGING_MARKETING_CODE"
                                                OnSelectedIndexChanging="GV_SELECT_MARKETING_CODE">
                                                <Columns>
                                                    <asp:BoundField DataField="T74CDE" HeaderText="Code" ReadOnly="True" SortExpression="T74CDE">
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="T74NME" HeaderText="Name" SortExpression="T74NME" />
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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
                            <%-- Popup Product Model --%>
                            <dx:ASPxPopupControl ID="PopupAddProductModel" ClientInstanceName="PopupAddProductModel" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Product Model" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="900px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif"></LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top"></ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label95" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="80px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label96" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlPopupAddProductModelSearchBy" runat="server" Width="150px">
                                                        <asp:ListItem Value="PT" Selected="True">Product Type Code</asp:ListItem>
                                                        <asp:ListItem Value="PC">Product Code</asp:ListItem>
                                                        <asp:ListItem Value="BC">Brand Code</asp:ListItem>
                                                        <asp:ListItem Value="MC">Model Code</asp:ListItem>
                                                        <asp:ListItem Value="MD">Model Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="Label97" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Select Product Model" Width="150px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label98" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 220px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupAddProductModelSearchText" runat="server" Width="200px"></asp:TextBox>&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;" colspan="2">
                                                    <asp:Button ID="btnPopupAddProductModelSearch" runat="server" Text="Search" Width="80px"
                                                        Height="24px" BackColor="#66CCFF" OnClick="btnPopupAddProductModelSearchClick" />&nbsp;&nbsp;
                                                    <asp:Button ID="btnPopupAddProductModelClear" runat="server" Text="Clear" Width="80px" Height="24px"
                                                        BackColor="#66CCFF" OnClick="btnPopupAddProductModelClearClick" />
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
                                                    <asp:BoundField DataField="T43TYP" HeaderText="Product Type" ReadOnly="True" SortExpression="T43TYP">
                                                        <ItemStyle Width="80px" HorizontalAlign="right" />
                                                        <ItemStyle Width="95px" />
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
                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
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

                            <%-- Popup ShareSub --%>
                            <dx:ASPxPopupControl ID="PopupShareSub" ClientInstanceName="PopupShareSub" ShowPageScrollbarWhenModal="true"
                                runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                HeaderText="Share Sub by Item" CloseAction="CloseButton" AllowDragging="True" AutoUpdatePosition="True"
                                Modal="True" Width="850px" Height="500px" CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css"
                                CssPostfix="BlackGlass" SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">
                                <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
                                </LoadingPanelImage>
                                <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
                                </ContentStyle>
                                <HeaderStyle>
                                    <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" PaddingTop="3px" />
                                    <Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
                                </HeaderStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl22" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:Label ID="lbPopupShareSubSubSeq" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Share Sub Seq" Width="200px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupShareSubSubSeq" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 150px; height: 24px;">
                                                    <asp:Label ID="lbPopupShareSubTotalTerm" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Share Sub Total Term" Width="150px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupShareSubTotalTerm" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:Label ID="Label60" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Share Sub Term Range" Width="200px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:TextBox ID="txtPopupShareSubTermRange" runat="server" Width="70px" MaxLength="5" Enabled="false" OnTextChanged="txtPopupShareSubTermRange_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="pPopupShareSubSetTerm" runat="server" visible="true">
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:Label ID="lbPopupShareSubTermofRange" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Share Sub Range of Term" Width="200px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <dx:ASPxTextBox ID="txtPopupShareSubTermofRange" runat="server" Width="70px" MaxLength="5" AutoPostBack="true"
                                                        OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtPopupShareSubTermofRange_TextChanged">
                                                        <ValidationSettings Display="Dynamic" ValidationGroup="gSubSeq">
                                                            <RequiredField ErrorText="* Required." IsRequired="true" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr id="pPopupShareSubStartEndTerm" runat="server" visible="false">
                                                <td align="left" width="250px" height="24px">
                                                    <asp:Label ID="Label66" runat="server" Text="Start Term"></asp:Label>
                                                </td>
                                                <td align="left" height="24px">
                                                    <table width="500px">
                                                        <tr>
                                                            <td align="left" width="100px" height="24px">
                                                                <dx:ASPxTextBox ID="txtPopupShareSubStartTerm" runat="server" Width="70px" Height="24px" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtPopupShareSubStartTerm_TextChanged" Enabled="false">
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                        <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td align="left" width="120px" height="24px">
                                                                <asp:Label ID="Label67" runat="server" Text="End Term" Width="120px"></asp:Label>
                                                            </td>
                                                            <td align="left" width="150px" height="24px">
                                                                <dx:ASPxTextBox ID="txtPopupShareSubEndTerm" runat="server" Width="70px" Height="24px" Enabled="false" MaxLength="5" AutoPostBack="true" OnKeyPress="return chkNumberOnly(this)" OnTextChanged="txtPopupShareSubEndTerm_TextChanged">
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="InsertTerm">
                                                                        <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td align="left" width="100px" height="24px">&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="pPopupShareSubRate" runat="server" visible="false">
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;">
                                                    <asp:Label ID="Label61" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Share Sub Rate of Range" Width="200px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 400px; height: 24px;">
                                                    <table width="400px">
                                                        <tr>
                                                            <td align="left" width="100px" height="24px">
                                                                <asp:Label ID="Label62" runat="server" Text="Customer Rate"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                                <dx:ASPxTextBox ID="txtPopupShareSubCustRate" runat="server" Width="70px" Height="24px" MaxLength="5"
                                                                    AutoPostBack="true" OnKeyPress="return chkNumberMinus(this)" OnTextChanged="txtPopupShareSubCustRate_TextChanged">
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="PopupShareSubInsertTerm">
                                                                        <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="rShareSubVendor" runat="server">
                                                            <td align="left" width="100px" height="24px">
                                                                <asp:Label ID="Label63" runat="server" Text="Sub rate Vendor"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                                <dx:ASPxTextBox ID="txtPopupShareSubVendorRate" runat="server" Width="70px" Height="24px" MaxLength="5"
                                                                    AutoPostBack="true" OnKeyPress="return chkNumber(this)" OnTextChanged="txtPopupShareSubVendorRate_TextChanged">
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="PopupShareSubInsertTerm">
                                                                        <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="rShareSubMaker" runat="server">
                                                            <td align="left" width="100px" height="24px">
                                                                <asp:Label ID="Label64" runat="server" Text="Sub rate Maker"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                                <dx:ASPxTextBox ID="txtPopupShareSubMakerRate" runat="server" Width="70px" Height="24px" MaxLength="5"
                                                                    AutoPostBack="true" OnKeyPress="return chkNumber(this)" OnTextChanged="txtPopupShareSubMakerRate_TextChanged">
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="PopupShareSubInsertTerm">
                                                                        <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="rShareSubESB" runat="server">
                                                            <td align="left" width="100px" height="24px">
                                                                <asp:Label ID="Label65" runat="server" Text="Sub rate ESB"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                                <dx:ASPxTextBox ID="txtPopupShareSubESBRate" runat="server" Width="70px" Height="24px" MaxLength="5"
                                                                    AutoPostBack="true" OnKeyPress="return chkNumber(this)" OnTextChanged="txtPopupShareSubESBRate_TextChanged">
                                                                    <ValidationSettings Display="Dynamic" ValidationGroup="PopupShareSubInsertTerm">
                                                                        <RequiredField ErrorText="* Required." IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="pPopupShareSubInputRate" runat="server" visible="false">
                                                <td align="center" width="100%" colspan="4">
                                                    <dx:ASPxButton ID="btnPopupShareSubInsert" runat="server" Text="Add Share Sub" Width="150px" ValidationGroup="PopupShareSubInsertTerm" OnClick="btnPopupShareSubInsert_Click">
                                                        <Image Url="~\Images\icon\success.png" Width="16"></Image>
                                                        <DisabledStyle CssClass="imgGreyscal"></DisabledStyle>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr id="pPopupShareSubGrid" runat="server" visible="false">
                                                <td colspan="2">
                                                    <hr style="margin-top: 10px; margin-bottom: 10px;" />
                                                    <asp:GridView ID="gvPopupShareSub" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" AllowPaging="true" PageSize="10"
                                                        EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="100%"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found"
                                                        OnPageIndexChanging="gvPopupShareSub_PageIndexChanging">
                                                        <Columns>
                                                            <asp:BoundField DataField="RATES" HeaderText="Rate" SortExpression="RATES">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="C05STO" HeaderText="Total Term" SortExpression="C05STO">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="C05CSQ" HeaderText="SubSeq" SortExpression="C05CSQ">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="C05RSQ" HeaderText="Seq" SortExpression="C05RSQ">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="C05STM" HeaderText="Term" SortExpression="C05STM">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="C05SBT" HeaderText="Type" SortExpression="C05SBT">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Easybuy" HeaderText="Easybuy" SortExpression="Easybuy">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ESubRate" HeaderText="E.Sub Rate" SortExpression="ESubRate">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Maker" HeaderText="Maker" SortExpression="Maker">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MSubRate" HeaderText="M.Sub Rate" SortExpression="MSubRate">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VSubRate" HeaderText="V.Sub Rate" SortExpression="VSubRate">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CampaignStartTerm" HeaderText="CampaignStartTerm" SortExpression="CampaignStartTerm">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CampaignEndTerm" HeaderText="CampaignEndTerm" SortExpression="CampaignEndTerm">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <EmptyDataRowStyle BackColor="#A3A3A3" ForeColor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                        <RowStyle BackColor="#F7F7DE" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="4" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    </asp:GridView>
                                                    <hr style="margin-top: 10px; margin-bottom: 10px;" />
                                                </td>
                                            </tr>
                                            <tr id="pPopupShareSubSubmit" runat="server" visible="false">
                                                <td align="right" width="100%" colspan="4">
                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" ValidationGroup="false" Text="Submit" Width="100px" OnClick="btnPopupShareSubOK_Click">
                                                        <ClientSideEvents Click="function(s, e) { PopupShareSub.Hide(); }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <dx:ASPxPopupControl ID="popupAlert_2" ClientInstanceName="popupAlert_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl26" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/Images/icon/alert.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <dx:ASPxButton ID="btnPopupAler" runat="server" Text="OK" Width="100px">

                                                        <ClientSideEvents Click="function(s, e) { popupAlert_2.Hide(); }"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>

                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup AlertApp --%>
                            <dx:ASPxPopupControl ID="PopupAlertApp_2" ClientInstanceName="PopupAlertApp_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl24" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/icon/question.png" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <dx:ASPxLabel ID="lblMsgAlertApp_2" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>


                                        <br />
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="100%">
                                               <dx:ASPxButton ID="ASPxButton2" runat="server" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupAlertApp_2.Hide(); }" />
                                                        
                                                    </dx:ASPxButton>
                                                </td>
                                                
                                            </tr>

                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%-- Popup Msg --%>
                            <dx:ASPxPopupControl ID="PopupMsg_2" ClientInstanceName="PopupMsg_2" ShowPageScrollbarWhenModal="true"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl25" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center">
                                                    <asp:Image ID="Image10" runat="server" ImageUrl="~/Images/icon/alertRed.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                        </table>

                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <br />
                                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="OK" Width="100px">
                                                        <ClientSideEvents Click="function(s, e) { PopupMsg_2.Hide(); }" />
                                                        
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <dx:ASPxPopupControl ID="Popup_Campaign_2" ClientInstanceName="Popup_Campaign_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl26_2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label69_2" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label70" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchCampaign_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CCP" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DCP">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label71" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label72" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchCampaign_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchCampaignPopup_2" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_CAMPAIGN_POPUP_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button12" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_CAMPAIGN_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_Campaign_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="campaign_PageIndexChanging_2"
                                                OnSelectedIndexChanging="campaign_SelectedIndexChanging_2">
                                              
                                                <Columns>

                                                    <asp:BoundField DataField="C01CMP" HeaderText="Code" ReadOnly="True" SortExpression="C01CMP" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                        <ItemStyle Width="100px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="C01CNM" HeaderText="Description" ReadOnly="True" SortExpression="C01CNM" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                       
                                                    </asp:BoundField>
                                                  



                                                    <asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select" ButtonType="Image" ImageUrl="~\Images\icon\click_select.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>

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
                            <dx:ASPxPopupControl ID="Popup_ProductType_2" ClientInstanceName="Popup_ProductType_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl27_2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label73" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label81" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchProducttype_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CPT" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DPT">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label101" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label104" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProducttype_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Button3" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_TYPE_POPUP_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button7" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_PRODUCTTYPE_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_ProductType_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="productType_PageIndexChanging_2"
                                                OnSelectedIndexChanging="productType_SelectedIndexChanging_2">
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

                            <dx:ASPxPopupControl ID="Popup_ProductCode_2" ClientInstanceName="Popup_ProductCode_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl28_2" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label105" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label106" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchProductCode_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CPC" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DPC">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label107" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label108" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProductCode_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Button4" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_CODE_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button8" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_PRODUCTCODE_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_ProductCode_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="productCode_PageIndexChanging_2"
                                                OnSelectedIndexChanging="productCode_SelectedIndexChanging_2">
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
                            <dx:ASPxPopupControl ID="Popup_Brand_2" ClientInstanceName="Popup_ProductCode_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl29" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label109" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label110" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchBrands_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CB" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DB">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label111" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label112" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchBrands_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="searchingBrand_2" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_BRAND_POPUP_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button9" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_BRAND_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">
                                            <asp:GridView ID="gv_Brand_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="brand_PageIndexChanging_2"
                                                OnSelectedIndexChanging="brand_SelectedIndexChanging_2">
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
                            <dx:ASPxPopupControl ID="Popup_Model_2" ClientInstanceName="Popup_Model_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl30" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label113" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label114" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchModel_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CMD" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DMD">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label115" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label116" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchModel_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Button10" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_MODEL_POPUP_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button11" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_MODEL_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_Model_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="model_PageIndexChanging_2"
                                                OnSelectedIndexChanging="model_SelectedIndexChanging_2">
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
                            <dx:ASPxPopupControl ID="Popup_ProductItem_2" ClientInstanceName="Popup_ProductItem_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl31" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label117" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label118" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchProductItem_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CPI" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DPI">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="Label119" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label120" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchProductItem_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Button13" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_PRODUCT_ITEM_POPUP_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button14" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_PRODUCT_ITEM_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />

                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gv_productItem_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="productItem_PageIndexChanging_2"
                                                OnSelectedIndexChanging="productItem_SelectedIndexChanging_2">
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
                            <dx:ASPxPopupControl ID="Popup_Vendor_2" ClientInstanceName="Popup_Vendor_2"
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
                                    <dx:PopupControlContentControl ID="PopupControlContentControl32" runat="server" SupportsDisabledAttribute="True">
                                        <table width="100%">
                                            <tr>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 90px; height: 24px;">
                                                    <asp:Label ID="Label121" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Search By" Width="70px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label122" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 15px; width: 170px; height: 24px;">
                                                    <asp:DropDownList ID="ddlSearchVendor_2" runat="server" Width="150px" Height="24">
                                                        <asp:ListItem Value="CV" Selected="True">Code</asp:ListItem>

                                                        <asp:ListItem Value="DV">Description</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 100px; height: 24px;">
                                                    <asp:Label ID="lbl_SelectProductType_2" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700;"
                                                        Text="Text Search" Width="100px"></asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 5px; height: 24px;">
                                                    <asp:Label ID="Label123" runat="server" Style="font-family: Tahoma; font-size: small; font-weight: 700; text-align: center;"
                                                        Width="5px" Height="16px">:</asp:Label>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 190px; height: 24px;">
                                                    <asp:TextBox ID="txtSearchVendor_2" runat="server" Width="160px" Height="18"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left; font-family: Tahoma; font-size: small; width: 200px; height: 24px;"
                                                    colspan="2">
                                                    <asp:Button ID="Popup_searchVendor_2" runat="server" Text="Search" Width="80px" Height="24px" OnClick="CLICK_SEARCH_VENDOR_POPUP_2"
                                                        BackColor="#66CCFF" />&nbsp;&nbsp;
                                                    <asp:Button ID="Button15" runat="server" Text="Clear" Width="80px" Height="24px" BackColor="#66CCFF" OnClick="CLEAR_POPUP_VENDOR_2" />
                                                </td>
                                            </tr>
                                        </table>

                                        <br />
                                        <div style="width: 100%; height: 450px; overflow: scroll">


                                            <asp:GridView ID="gvVendor_2" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                PageSize="15" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                                                EnableModelValidation="True" ForeColor="Black" GridLines="Vertical" Width="99%"
                                                AllowPaging="True" EmptyDataText="No records found" OnPageIndexChanging="gvVendor_PageIndexChanging_2"
                                                OnSelectedIndexChanging="gvVendor_SelectedIndexChanging_2">
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

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>
</asp:Content>
