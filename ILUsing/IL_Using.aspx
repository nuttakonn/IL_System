<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="IL_Using.aspx.cs" Inherits="ManageData_WorkProcess_ChangeData_Received_Case_Using" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxSplitter" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/ManageData/WorkProcess/UserControl/UC_Product_Search.ascx" TagName="UC_ProductSearch" TagPrefix="uc1" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="phContents" Runat="Server">
<script type="text/javascript" src='<%=ResolveUrl("~/Js/shortcut.js")%>' ></script>
<script  type="text/javascript"  src='<%=ResolveUrl("~/Js/ProtectJs.js")%>' ></script>
    <script type="text/javascript">
        //        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //        prm.add_initializeRequest(InitializeRequest);
        //        prm.add_endRequest(EndRequest);

        //        function InitializeRequest(sender, args) {
        //        }

        //        function EndRequest(sender, args) {
        //            disableSelect(document.getElementById("<%=txt_birthDate_P.ClientID %>"));
        //            disableSelect(document.getElementById("<%=txt_telM.ClientID %>"));
        //        }

        // <![CDATA[
        function ShowDocType() {
            pop_DocScr.Show();
        }
        function HideDocType() {
            pop_DocScr.Hide();
        }
        function HidePopup() {
            pop_Vendor.Hide();
        }

        function hide_Password() {

            disableSelect(document.getElementById("<%=txt_birthDate_P.ClientID %>"));
        }


        function OnUniteChanged(dd_vendor) {
            txt_vendorBrn.SetText(dd_vendor.GetSelectedItem().GetColumnText('p10FI1'));
            dd_campaign.SetSelectedIndex(2);

        }

        function OnCampaignChange(dd_campaign) {
            txt_campSeq.SetText(dd_campaign.GetSelectedItem().GetColumnText('C02RSQ'));


        }
        function OnProductChange(dd_product) {
            txt_minPrice.SetText(dd_campaign.GetSelectedItem().GetColumnText('C02RSQ'));
            txt_maxPrice.SetText(dd_campaign.GetSelectedItem().GetColumnText('C02RSQ'));
        }

        function disableSelect(el) {

            if (el.addEventListener) {
                el.addEventListener("mousedown", disabler, "false");
            } else {
                el.attachEvent("onselectstart", disabler);
            }
        }

        function enableSelect(el) {
            if (el.addEventListener) {
                el.removeEventListener("mousedown", disabler, "false");
            } else {
                el.detachEvent("onselectstart", disabler);
            }
        }

        function disabler(e) {
            if (e.preventDefault) { e.preventDefault(); }
            return false;
        }


        function OnSumbitAll(s, e) {
            var groupsToValidate = ["v_card", "v_cal_prod", "v_SaveCr"];
            var success = true;
            for (var i = 0; i < groupsToValidate.length; i++)
                success = ASPxClientEdit.ValidateGroup(groupsToValidate[i]) && success;
            e.processOnServer &= success;
        }





        //        window.onload = function () {
        //            disableSelect(document.getElementById("<%=txt_birthDate_P.ClientID %>"));
        //            disableSelect(document.getElementById("<%=txt_telM.ClientID %>"));
        //        };

        // ]]> 
    </script>
    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 23px;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    GroupBoxCaptionOffsetY="-28px" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100%" 
                    Height="136px">
                    <ContentPaddings Padding="5px" />
<ContentPaddings Padding="5px"></ContentPaddings>
                    <HeaderTemplate>
                        <span style="font-size: small">Approve by using card</span>
                    </HeaderTemplate>
                    <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <table style="border: thin dotted #CCCCCC; width: 100%">
        <tr>
            <td style="width: 163px; height: 18px; width:103px;">
                <asp:Label ID="Label5" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Card number :" Width="103px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px;width:150px; ">
               <div style="float:left;">
                <dx:ASPxTextBox ID="txt_card_no" runat="server" Width="150px" Height="24px" 
                       TabIndex="1">
<MaskSettings Mask="0000-0000-0000-0000"></MaskSettings>

                <ValidationSettings ValidationGroup="v_card" >
                    <RequiredField IsRequired="true" ErrorText="" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                </ValidationSettings>
                </dx:ASPxTextBox>
               </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px;width:250px;">
               <div style="float:left;">
                <dx:ASPxButton ID="btn_search" runat="server" Text="Search" Width="100px" Height="18px" 
                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                OnClick="btn_search_Click" CausesValidation="true" ValidationGroup="v_card" 
                       TabIndex="1" >
               
<ClientSideEvents Click="function(s, e) {ASPxClientEdit.ValidateGroup(&#39;v_card&#39;);
                Callback.PerformCallback();
                LoadingPanel.Show(); }"></ClientSideEvents>
                 </dx:ASPxButton>
               </div>
               <div style="float:left">
                     &nbsp; &nbsp; 
                </div>
               <div style="float:left;">
                <dx:ASPxButton ID="btn_clear" runat="server" Text="Clear" Width="100px" Height="18px" 
                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                 SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                       OnClick="btn_clear_Click" ValidationGroup="v_clear">
                 </dx:ASPxButton>
               </div>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px;width:350px;">
                <div style="float:left">
                     <asp:Label  runat="server" ID="lb_nameCust" Width="270px" 
                     style="font-family: Tahoma; font-size: medium;  
                     font-weight: 600;" Text="" ForeColor="#3366CC">
                     </asp:Label>
                </div>
                <div style="float:left">
                     &nbsp; &nbsp;
                </div>
                <div style="float:left">
                     <asp:Label  runat="server" ID="lb_csn" Width="40px" 
                     style="font-family: Tahoma; font-size: medium;  
                     font-weight: 500;" Text="" ForeColor="#3366CC">
                     </asp:Label>
                    
                </div>
            </td>
            
            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; width:150px;">
                <asp:Label ID="lb_rescard" runat="server" Width="180px"  
                Height="18px" Font-Size="Medium" ForeColor="Red" Font-Bold="true" 
                    BackColor="White">
                </asp:Label>
            </td>
        </tr>
    </table>
    <dx:ASPxSplitter runat="server" ID="SP_1" Width="100%" Height="1500px" Orientation="Vertical">
        <Panes>
             <dx:SplitterPane ScrollBars="Vertical" Size="50px"  ShowCollapseBackwardButton="True">
<Separator>
<ButtonStyle>
<Border BorderColor="Aqua"></Border>
</ButtonStyle>
</Separator>
            <ContentCollection>
                <dx:SplitterContentControl ID="SplitterContentControl1" runat="server" Height="50px">
                    <table  style="width: 100%" >
                        <tr>
                            <td style="width: 120px; height: 18px;">
                                <asp:Label ID="Label1" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Application date :" Width="120px"></asp:Label>
                            </td>
                            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; ">
                                <div style="float:left;">
                                    <dx:ASPxTextBox ID="txt_appDate" runat="server" Width="100px" Height="24px" 
                                        BackColor="#FFFFC4">
                                         <MaskSettings Mask="00/00/0000"></MaskSettings>
                                            <ValidationSettings ValidationGroup="v_calACL" >
                                                <RequiredField IsRequired="true" ErrorText="กรุณาระบุ" />
<RequiredField IsRequired="True" ErrorText="กรุณาระบุ"></RequiredField>
                                            </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </div>
                                <div style="float:left;">&nbsp;</div>
                                <div style="float:left;">
                                    <asp:Label ID="Label2" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="ID Number. :" Width="103px"></asp:Label>
                                </div>
                                <div style="float:left;">
                                    <dx:ASPxTextBox ID="txt_idNo" runat="server" Width="100px" Height="24px" Enabled="false">
                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                    </DisabledStyle>
                                    </dx:ASPxTextBox>
                                </div>
                            </td>
                           
                            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; width:100px;">
                                <asp:Label ID="Label3" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Expire date :" Width="90px"></asp:Label>
                                <%--<asp:Label ID="Label4" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Birth date :" Width="100px"></asp:Label>--%>
                               
                            </td>
                            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px;width:250px;">
                                <div style="float:left;">
                                    <dx:ASPxTextBox ID="txt_expireDate" runat="server" Width="100px" Height="24px" Enabled="false">
                                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                        </DisabledStyle>
                               
                                    </dx:ASPxTextBox>
                                </div>
                               
                            </td>
                         
                            <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; " colspan="2">
                                 <dx:ASPxButton ID="btnNote" runat="server" AutoPostBack="true" 
                                     CausesValidation="false" Text="Add Note" Width="70px" Wrap="False"
                                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                                 SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                     OnClick="btnNote_Click" Font-Bold="True">
                                     <ClientSideEvents Click="function(s, e) {
                                            Callback.PerformCallback();LoadingPanel.Show(); }">
                                     </ClientSideEvents>
                                 </dx:ASPxButton>
                             </td>
                          
                        </tr>
                    </table>
                </dx:SplitterContentControl>
            </ContentCollection>
        </dx:SplitterPane>
      
        <dx:SplitterPane ScrollBars="Vertical">
            <ContentCollection>
                <dx:SplitterContentControl ID="SplitterContentControl2" runat="server" Height="700px">
                           <dx:ASPxPageControl Width="100%" Height="420px" ID="tabDetail" runat="server" 
                                ActiveTabIndex="1" EnableHierarchyRecreation="True" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css">
                            <TabPages>  
                <dx:TabPage Text="Judgment Form" Name="TabJudgment">
                    <TabStyle Font-Bold="True"></TabStyle>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl1_" runat="server" SupportsDisabledAttribute="True">
                                <asp:Panel ID="pl_cust" runat="server">
                                <table  style="border:thin dotted   #87CEFA; width: 100%; " >
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label13" runat="server" 
                                             style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                             Text="1. Birth date(ววดดปปปป) " Width="100px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                         <dx:ASPxTextBox ID="txt_birthDate_P" runat="server" Width="130px" 
                                             Height="24px"  MaxLength="8" 
                                                ClientInstanceName="txt_birthDate_P" ForeColor="Black" 
                                                ToolTip="ววดดปปปป" BackColor="#FFFFC4" Password="True" TabIndex="2">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>  
                                                <MaskSettings Mask="00000000"></MaskSettings>

                                             <ValidationSettings ValidationGroup="v_calACL" SetFocusOnError="True" 
                                                    ErrorText="" >
                                                <RequiredField IsRequired="true" ErrorText="" />
                                                 <RegularExpression ErrorText="" />
                                                 <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                 <RegularExpression ErrorText=""></RegularExpression>
                                            </ValidationSettings>  
                                            </dx:ASPxTextBox>
                                            <%--<dx:ASPxTextBox ID="txt_birthDate_P" runat="server" Width="130px" 
                                             Height="24px"  MaxLength="8" BackColor="#FFFFC4" 
                                                ClientInstanceName="txt_birthDate_P" ForeColor="#FFFFC4" ToolTip="ววดดปปปป">  
                                                <MaskSettings Mask="00000000"></MaskSettings>

                                             <ValidationSettings ValidationGroup="v_calACL" SetFocusOnError="True" 
                                                    ErrorText="" >
                                                <RequiredField IsRequired="true" ErrorText="" />
                                                 <RegularExpression ErrorText="" />
                                                 <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                 <RegularExpression ErrorText=""></RegularExpression>
                                            </ValidationSettings>  
                                            </dx:ASPxTextBox>--%>
                                          <asp:HiddenField ID="hidSurname" runat="server" />
                                          <asp:HiddenField ID="hidGender" runat="server" />
                                          <asp:HiddenField ID="hidMobile" runat="server" />
                                          <asp:HiddenField ID="hidName" runat="server" />
                                          <asp:HiddenField ID="hidExt" runat="server" />
                                          <asp:HiddenField ID="hidSalary" runat="server" />

                                          <asp:HiddenField ID="hidZipcode" runat="server" />
                                          <asp:HiddenField ID="hidTambol" runat="server" />
                                          <asp:HiddenField ID="hidAmphur" runat="server" />
                                          <asp:HiddenField ID="hidProvince" runat="server" />

                                            <asp:HiddenField ID="hid_password" runat="server" />
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                             <asp:Label ID="Label24" runat="server" 
                                             style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                             Text="8. Occupation" Width="130px"></asp:Label>
                                           
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                             <div style="float:left;">
                                                <dx:ASPxComboBox ID="dd_occup" runat="server" Width="200px" TabIndex="13" 
                                                    AutoPostBack="True" OnSelectedIndexChanged="dd_occup_SelectedIndexChanged">
                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                      <ValidationSettings SetFocusOnError="True" ValidationGroup="v_calACL" 
                                                        ErrorText="">
                                                     <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                           </div>
                                           <div style="float:left;">
                                           &nbsp;
                                           </div>
                                            <div style="float:left;">
                                                <asp:Label ID="lb_occup" runat="server" ForeColor="red" Font-Size="Small" 
                                                    Font-Bold="true" Visible="False"></asp:Label>
                                           </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label104" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="1.1 Birth date (confirm) " Width="180px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                           
                                                <dx:ASPxTextBox ID="txt_birthDate_C1" runat="server" BackColor="#FFFFC4" 
                                                    Height="24px" MaxLength="8" Width="130px" ToolTip="ววดดปปปป" TabIndex="3">
                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle> 
                                                    <MaskSettings Mask="00/00/0000"/>
<MaskSettings Mask="00/00/0000"></MaskSettings>

                                                    <ValidationSettings ErrorText="" SetFocusOnError="True" 
                                                        ValidationGroup="v_calACL">
                                                        <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                       
                                            <asp:HiddenField ID="hid_birthDate" runat="server" />
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label103" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="Commercial Regis" Width="130px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                            <dx:ASPxComboBox ID="dd_comerc" runat="server" Width="200px" TabIndex="14" 
                                                AutoPostBack="True" OnSelectedIndexChanged="dd_comerc_SelectedIndexChanged">
                                                      <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                      <ValidationSettings SetFocusOnError="True" ValidationGroup="v_calACL" 
                                                        ErrorText="">
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label105" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="2. Telephone (Mobile) " Width="150px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                            <dx:ASPxRadioButtonList ID="rb_mobile" runat="server" 
                                                RepeatDirection="Horizontal" TabIndex="4" Width="150px" >
                                                
                                                <Items>
                                                    <dx:ListEditItem Text="Yes" Value="Y" />
                                                    <dx:ListEditItem Text="No" Value="N" />
                                                </Items>
                                                <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                    <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxRadioButtonList>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                              <asp:Label ID="Label26" runat="server" 
                                             style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                             Text="9. Position" Width="100px"></asp:Label>
                                            
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                              <div style="float:left;">
                                             <dx:ASPxComboBox ID="dd_position" runat="server" Width="200px" TabIndex="15">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="v_calACL" 
                                                    ErrorText="">
                                                 <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                             </div>
                                             <div style="float:left;">
                                           &nbsp;
                                           </div>
                                              <div style="float:left;">
                                             
                                         
                                             <asp:Label ID="lb_position" runat="server" ForeColor="red" Font-Size="Small" 
                                                      Font-Bold="true" Visible="False"></asp:Label>
                                             </div>
                                             
                                         
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label106" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="2.1 Mobile No. " Width="130px"></asp:Label>
                                          
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                             <dx:ASPxTextBox ID="txt_telM" runat="server" BackColor="#FFFFC4" 
                                                 ClientInstanceName="txt_telM" ForeColor="Black" Width="150px" 
                                                 AutoCompleteType="Disabled" MaxLength="10" Password="True" TabIndex="5">
                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                 <MaskSettings AllowMouseWheel="False" Mask="9999999999" />
                                                 <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                     <RequiredField ErrorText="" />
<RequiredField ErrorText=""></RequiredField>
                                                 </ValidationSettings>
                                                 <DisabledStyle BackColor="Silver">
                                                 </DisabledStyle>
                                             </dx:ASPxTextBox>
                                             <asp:HiddenField ID="hid_mobile" runat="server" />
                                         
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                             <asp:Label ID="Label27" runat="server" 
                                             style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                             Text="10. Total of employee " Width="150px"></asp:Label>
                                           
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                             <dx:ASPxTextBox ID="txt_empNo" runat="server" Width="150px" TabIndex="16">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                 <MaskSettings AllowMouseWheel="False" Mask="99999" />
                                            </dx:ASPxTextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                             <asp:Label ID="Label107" runat="server" 
                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                 Text="2.2 Mobile No.(Confirm) " Width="180px"></asp:Label>
                                           
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                             <div style="float:left;">
                                           </div>
                                             <dx:ASPxTextBox ID="txt_telM_C" runat="server" BackColor="#FFFFC4" 
                                                 Width="150px" AutoCompleteType="Disabled" ClientInstanceName="txt_telM_C" 
                                                 MaxLength="10" TabIndex="6">
                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                 <MaskSettings AllowMouseWheel="False" Mask="9999999999" />
                                                 <ValidationSettings ValidationGroup="v_calACL">
                                                 </ValidationSettings>
                                                 <DisabledStyle BackColor="Silver">
                                                 </DisabledStyle>
                                             </dx:ASPxTextBox>
                                            
                                         
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                           <asp:Label ID="Label28" runat="server" 
                                             style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                             Text="11. Employee Type" Width="120px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                            <div style="float:left;">
                                            <dx:ASPxComboBox ID="dd_empType" runat="server" Width="200px" TabIndex="17">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="v_calACL" 
                                                    ErrorText="">
                                                    <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                            </div>
                                            <div style="float:left;">
                                           &nbsp;
                                           </div>
                                             <div style="float:left;">
                                            <asp:Label ID="lb_empType" runat="server" ForeColor="red" Font-Size="Small" 
                                                     Font-Bold="true" Visible="False"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                             <asp:Label ID="Label108" runat="server" 
                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                 Text="3. Marital status" Width="110px"></asp:Label>
                                       
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                              <div style="float:left;">
                                             <dx:ASPxComboBox ID="dd_marital" runat="server" TabIndex="7" 
                                                 ValueType="System.String" Width="200px">
                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                 <ValidationSettings ErrorText="" SetFocusOnError="True" 
                                                     ValidationGroup="v_calACL">
                                                     <RequiredField ErrorText="" IsRequired="True" />
                                                 </ValidationSettings>
                                             </dx:ASPxComboBox>
                                            </div>
                                            <div style="float:left;">
                                           &nbsp;
                                           </div>
                                            <div style="float:left;">
                                             <asp:Label ID="lb_marital" runat="server" Text="" ForeColor="red" 
                                                    Font-Size="Small" Font-Bold="true" Visible="False"></asp:Label>
                                             </div>
                                        
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label29" runat="server" 
                                             style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                             Text="12. Length of service" Width="150px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                             <div style="float:left;">
                                                <dx:ASPxTextBox ID="txt_service_Y" runat="server" Width="30px" MaxLength="2" 
                                                     TabIndex="18" >
                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                   <MaskSettings Mask="09"></MaskSettings>

                                                    <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                        <RegularExpression ErrorText="" />
                                                        <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>

<RegularExpression ErrorText=""></RegularExpression>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div style="float:left;">
                                          
                                                <asp:Label ID="Label32" runat="server" 
                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                 Text="Year" Width="40px"></asp:Label>
                                             </div>
                                            <div style="float:left;">
                                                <dx:ASPxTextBox ID="txt_service_M" runat="server" Width="30px" MaxLength="2" 
                                                    TabIndex="19" >
                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                 
<MaskSettings Mask="09"></MaskSettings>

                                                    <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                      
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div style="float:left;">
                                           
                                                <asp:Label ID="Label31" runat="server" 
                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                 Text="Month" Width="40px"></asp:Label>
                                             </div>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label109" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="4. Type of resident" Width="130px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                             <div style="float:left;">
                                            <dx:ASPxComboBox ID="dd_resident" runat="server" TabIndex="8" 
                                                ValueType="System.String" Width="200px">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                <ValidationSettings ErrorText="" SetFocusOnError="True" 
                                                    ValidationGroup="v_calACL">
                                                    <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                            </div>
                                            <div style="float:left;">
                                           &nbsp;
                                           </div>
                                             <div style="float:left;">
                                            <asp:Label ID="lb_resident" runat="server" ForeColor="red" Font-Size="Small" 
                                                     Font-Bold="true" Visible="False"></asp:Label>
                                            </div>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            
                                            <asp:Label ID="Label102" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="13. Salary" Width="150px"></asp:Label>
                                            
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                             <dx:ASPxTextBox ID="txt_salary" runat="server" Width="150px" TabIndex="20">
                                             <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                             <MaskSettings Mask="&lt;0..99999999g&gt;.&lt;00..99&gt;"></MaskSettings>

                                               <ValidationSettings ErrorText="" ValidationGroup="v_calACL">        
                                                    <RequiredField ErrorText=""></RequiredField>

                                                    <RegularExpression ErrorText=""></RegularExpression>
                                               </ValidationSettings>
                                            </dx:ASPxTextBox>
                                           
                                           
                                             <asp:HiddenField ID="hid_m00sal" runat="server" />
                                           
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label110" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="5. Total of family" Width="110px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                             <dx:ASPxTextBox ID="txt_fPerson" runat="server" TabIndex="9" Width="100px">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                 <MaskSettings Mask="99" />
                                                 <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                     <RequiredField ErrorText="" IsRequired="True" />
                                                 </ValidationSettings>
                                             </dx:ASPxTextBox>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                          
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                            <div style="float:left;">
                                             <dx:ASPxButton ID="btn_calculate" runat="server" 
                                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                CssPostfix="Office2003Blue" OnClick="btn_calculate_Click" 
                                                SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                                Text="Calculate ACL" ValidationGroup="v_calACL" Width="95px" TabIndex="21">
                                            </dx:ASPxButton>
                                            </div>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            <asp:Label ID="Label111" runat="server" 
                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                Text="6. Period of time resident" Width="170px"></asp:Label>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                            
                                            <table style="text-align:left;" width="100%px">
                                                <tr>
                                                    <td style="width:35px;text-align:left;">
                                                        <dx:ASPxTextBox ID="txt_yearResident" runat="server" MaxLength="2" TabIndex="10" 
                                                            Width="30px">
                                                            <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                            <MaskSettings Mask="09" />
<MaskSettings Mask="09"></MaskSettings>

                                                            <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                                <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td style="width:35px;text-align:left;">
                                                        <asp:Label ID="Label112" runat="server" 
                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Year" 
                                                            Width="30px"></asp:Label>
                                                    </td>
                                                    <td style="width:35px;text-align:left;">
                                                        <dx:ASPxTextBox ID="txt_monthResident" runat="server" MaxLength="2" 
                                                            TabIndex="11" Width="30px">
                                                            <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                            <MaskSettings Mask="09" />
<MaskSettings Mask="09"></MaskSettings>

                                                            <ValidationSettings ErrorText="" ValidationGroup="v_calACL">
                                                                <RequiredField ErrorText="" />
<RequiredField ErrorText=""></RequiredField>
                                                            </ValidationSettings>
                                                        </dx:ASPxTextBox>
                                                    </td>
                                                    <td style="width:35px;text-align:left;">
                                                        <asp:Label ID="Label113" runat="server" 
                                                            style="font-family: Tahoma; font-size: small; font-weight: 700;" Text=" Month" 
                                                            Width="30px"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                           
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                           
                                            <asp:HiddenField ID="hid_date_97" runat="server" />
                                            <asp:HiddenField ID="hid_date_sal_old" runat="server" />
                                            <asp:HiddenField ID="hid_date_sal_old_d" runat="server" />
                                            <asp:HiddenField ID="hid_date_sal_old_t" runat="server" />
                                           
                                            <asp:HiddenField ID="hid_m11Tel" runat="server" />
                                            <asp:HiddenField ID="hid_m00oft" runat="server" />
                                            <asp:HiddenField ID="hid_m00ofc" runat="server" />
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                             <asp:Label ID="Label23_0" runat="server" 
                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                 Text="7. Type of Business " Width="130px"></asp:Label>
                                        </td> 
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                            <div style="float:left;">
                                            <dx:ASPxComboBox ID="dd_busType" runat="server" TabIndex="12" 
                                                ValueType="System.String" Width="200px">
                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF"></DisabledStyle>
                                                <ValidationSettings ErrorText="" SetFocusOnError="True" 
                                                    ValidationGroup="v_calACL">
                                                    <RequiredField ErrorText="" IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                            </div>
                                            <div style="float:left;">
                                           &nbsp;
                                           </div>
                                            <div style="float:left;">
                                            <asp:Label ID="lb_busType" runat="server"  ForeColor="red" Font-Size="Small" Font-Bold="true"></asp:Label>
                                            </div>
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                         
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                         
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            &nbsp;</td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:345px; height:24px;">
                                            &nbsp;</td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                            
                                        </td>
                                        <td style="text-align: left; font-family: Tahoma; font-size: small; width:350px;height:24px;">
                                          
                                        </td>
                                    </tr>
                                    
                                </table>
                                </asp:Panel>
                            </dx:ContentControl>
                        </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="TCL Result & Product" Name="TabTCL">
                    <TabStyle Font-Bold="True"></TabStyle>
                        <ContentCollection>
                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                   <dx:ASPxSplitter runat="server" ID="ASPxSplitter1" Width="100%" Height="1200px" Orientation="Vertical">
                                    <Panes>
                                        <dx:SplitterPane ScrollBars="Vertical" Size="75px"  ShowCollapseBackwardButton="True" Name="P_TCL">
                                        <Separator>
                                            <ButtonStyle><Border BorderColor="Aqua"></Border></ButtonStyle></Separator>
                                            <ContentCollection>
                                                <dx:SplitterContentControl ID="SplitterContentControl3" runat="server" Height="75px" BackColor="#87CEFA"> 
                                                    <table  style="width: 100%; " >
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:170px;height:24px;">
                                                                <asp:Label ID="Label55" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="EBC Limit " Width="130px"></asp:Label>
                                                                <%--<asp:Label ID="Label33" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Auto credit Line (Existing) " Width="170px"></asp:Label>--%>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_ebcLimit" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="false"  >
                                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                </dx:ASPxTextBox>
                                                                <%--<dx:ASPxTextBox ID="txt_autoCr" runat="server" Width="130px" 
                                                                 Height="24px">   
                                                                </dx:ASPxTextBox>--%>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                <asp:Label ID="Label56" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Total credit Line (Existing)" Width="180px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                 <dx:ASPxTextBox ID="txt_total_crt" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="false">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                 </dx:ASPxTextBox>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                <asp:Label ID="Label57" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Customer balance" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               <dx:ASPxTextBox ID="txt_cust_bln" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="false">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:170px;height:24px;">
                                                                 <asp:Label ID="Label58" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="TCL Available" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                 <div style="float:left;">
                                                                    <dx:ASPxTextBox ID="txt_tcl" runat="server" Width="90px" 
                                                                     Height="24px"  Enabled="false">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;
                                                                </div>
                                                                <div style="float:left;">
                                                                    <dx:ASPxTextBox ID="txt_app_lm" runat="server" Width="90px" 
                                                                     Height="24px"  Enabled="false">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                               <asp:Label ID="Label60" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="BOT Loan" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               <dx:ASPxTextBox ID="txt_bot_loan" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="false"> 
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>  
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                               <asp:Label ID="Label61" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="BOT Credit Available" Width="130px"></asp:Label>
                                                               <%-- <asp:Label ID="Label59" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Total credit Line (Existing)" Width="180px"></asp:Label>--%>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               <div style="float:left;">
                                                                    <dx:ASPxTextBox ID="txt_bot_crA" runat="server" Width="130px" 
                                                                     Height="24px"  Enabled="false">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;
                                                                </div>
                                                               <%-- <dx:ASPxTextBox ID="ASPxTextBox27" runat="server" Width="130px" 
                                                                 Height="24px"></dx:ASPxTextBox>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:170px;height:24px;">
                                                                <asp:Label ID="Label65" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Income" Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               <asp:Label ID="lb_pIncome" runat="server" Width="130px"  style="font-family: Tahoma; font-size: small; font-weight: 700;"   ForeColor="White">   
                                                               </asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                               <asp:Label ID="Label67" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Approve available" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               <asp:Label ID="lb_pApproveA" runat="server" Width="130px" style="font-family: Tahoma; font-size: small; font-weight: 700;"  ForeColor="White">   
                                                                </asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:170px;height:24px;">
                                                                <div style="float:left;">
                                                                    <asp:Label ID="Label62" runat="server" 
                                                                     style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                     Text="Group no." Width="130px"></asp:Label>
                                                                 </div>
                                                                 
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                <div style="float:left;">
                                                                      <dx:ASPxTextBox ID="txt_group" runat="server" Width="130px" 
                                                                     Height="24px" Enabled="false">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                     </dx:ASPxTextBox>
                                                                 </div>
                                                              
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                 <div style="float:left;">
                                                                <asp:Label ID="Label63" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Rank ." Width="130px"></asp:Label>
                                                               </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                                <div style="float:left;">
                                                                <dx:ASPxTextBox ID="txt_rank" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="false">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                               </div>
                                                               
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                                                               &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:SplitterContentControl>
                                            </ContentCollection>
                                         </dx:SplitterPane>
                                         <dx:SplitterPane ScrollBars="Vertical" Size="430px"  ShowCollapseBackwardButton="True">
                                        <Separator>
                                            <ButtonStyle><Border BorderColor="Aqua"></Border></ButtonStyle></Separator>
                                            <ContentCollection>
                                                <dx:SplitterContentControl ID="SplitterContentControl4" runat="server" Height="430px" Width="98%" BackColor="#B0E0E6">

                                                    <table width="100%" style="background-color:Yellow;">
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                <asp:Label ID="Label94" runat="server" 
                                                                 style="font-family: Tahoma; font-size: medium; font-weight: 700;" 
                                                                 Text="Payment Type" Width="130px" Font-Underline="true" ForeColor="Blue"></asp:Label>
                                                            </td> 
                                                        </tr>
                                                    </table> 
                                                    <table  style="width: 100%; " >
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                                                                <asp:Label ID="Label95" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Payment type " Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:200px;height:24px; ">
                                                                 <div style="float:left;">
                                                                <dx:ASPxComboBox ID="dd_paymentType" runat="server" Width="150px" 
                                                                    OnSelectedIndexChanged="dd_paymentType_SelectedIndexChanged" AutoPostBack="true">
                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {Callback.PerformCallback(); LoadingPanel.Show();}" />
<ClientSideEvents SelectedIndexChanged="function(s, e) {Callback.PerformCallback(); LoadingPanel.Show();}"></ClientSideEvents>
                                                                </dx:ASPxComboBox>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:100px;height:24px; ">
                                                              <asp:Label ID="Label101" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Reason code " Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:500px;height:24px; ">
                                                              <div style="float:left;">
                                                                <dx:ASPxComboBox ID="dd_reason" runat="server" Width="150px" Enabled="false" >
                                                                <Items>
                                                                    <dx:ListEditItem Value="A" Text="A:อนุมัติ" Selected="true" />
                                                                </Items>
                                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                </DisabledStyle> 
                                                              </dx:ASPxComboBox>
                                                              </div>
                                                              <div style="float:left;">&nbsp;&nbsp;</div>
                                                              <div style="float:left;">
                                                                <dx:ASPxButton ID="btn_saveCr" runat="server" 
                                                                 CausesValidation="true" 
                                                                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                                 CssPostfix="Office2003Blue" Height="18px" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                                                 Text="Save Credit" Width="100px" OnClick="btn_saveCr_Click" >
                                                                 <ClientSideEvents Click="OnSumbitAll"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                              </div>
                                                              <div style="float:left;">&nbsp;&nbsp;&nbsp;</div>
                                                              <div style="float:left;">
                                                                <dx:ASPxButton ID="btn_cancel_case" runat="server" 
                                                                 CausesValidation="true" 
                                                                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                                 CssPostfix="Office2003Blue" Height="18px" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                                                 Text="Cancel Case" Width="100px" OnClick="btn_cancel_case_Click"  >
                                                                </dx:ASPxButton>
                                                              </div>
                                                            </td>
                                                        </tr>
                                                   </table>
                                                        <asp:Panel ID="pl_payment" runat="server" Visible="false">
                                                        <table  style="width: 100%; ">
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                                                                <asp:Label ID="Label96" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Bank code " Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:300px;height:24px; ">
                                                                <div style="float:left;">
                                                                    <dx:ASPxComboBox ID="dd_bankCode" runat="server" Width="200px"  
                                                                        OnSelectedIndexChanged="dd_bankCode_SelectedIndexChanged" 
                                                                        AutoPostBack="True">
                                                                    
<ClientSideEvents SelectedIndexChanged="function(s, e) {Callback.PerformCallback(); LoadingPanel.Show();}"></ClientSideEvents>
                                                                    </dx:ASPxComboBox>
                                                                    <%--<dx:ASPxTextBox ID="txt_bankCode" runat="server" Width="100px" 
                                                                     Height="24px">   
                                                                    </dx:ASPxTextBox>--%>
                                                                </div>
                                                             
                                                                
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:100px;height:24px; ">
                                                                <asp:Label ID="Label97" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Branch code " Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:350px;height:24px; ">
                                                                <div style="float:left;">
                                                                    <dx:ASPxComboBox ID="dd_bankBranch" runat="server" Width="200px"></dx:ASPxComboBox>
                                                                    <%--<dx:ASPxTextBox ID="ASPxTextBox65" runat="server" Width="100px" 
                                                                     Height="24px">   
                                                                    </dx:ASPxTextBox>--%>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                    <%--<dx:ASPxTextBox ID="ASPxTextBox66" runat="server" Width="200px" 
                                                                     Height="24px">   
                                                                    </dx:ASPxTextBox>--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                                                                <asp:Label ID="Label98" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Account Type " Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:300px;height:24px; ">
                                                                <div style="float:left;">
                                                                    <dx:ASPxComboBox ID="dd_accountType" runat="server" Width="200px"></dx:ASPxComboBox>
                                                                    <%--<dx:ASPxTextBox ID="ASPxTextBox67" runat="server" Width="100px" 
                                                                     Height="24px">   
                                                                    </dx:ASPxTextBox>--%>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                    <%--<dx:ASPxTextBox ID="ASPxTextBox68" runat="server" Width="200px" 
                                                                     Height="24px">   
                                                                    </dx:ASPxTextBox>--%>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:100px;height:24px; ">
                                                                <asp:Label ID="Label99" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Account No. " Width="100px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:350px;height:24px; ">
                                                                <div style="float:left;">
                                                                    <dx:ASPxTextBox ID="txt_AccountNo" runat="server" Width="100px" 
                                                                     Height="24px" MaxLength="15">   
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>
                                                    <table width="100%" style="background-color:Yellow;">
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:130px;height:24px;">
                                                                <asp:Label ID="Label64" runat="server" 
                                                                 style="font-family: Tahoma; font-size: medium; font-weight: 700;" 
                                                                 Text="Product" Width="130px" Font-Underline="true" ForeColor="Blue"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                                                                
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                                                               
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                                                                <asp:Label ID="Label66" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Approve Limit" Width="130px" Visible="False"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                                                                <asp:Label ID="lb_pApproveL" runat="server" Width="130px"  
                                                                    style="font-family: Tahoma; font-size: small; font-weight: 700;"   
                                                                    ForeColor="Red" Visible="False"> </asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                                                                
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label68" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Total Term" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_totalterm" runat="server" AutoPostBack="true" 
                                                                    OnTextChanged="dd_totalterm_SelectedIndexChanged" TabIndex="17">
                                                                     <MaskSettings AllowMouseWheel="False" Mask="09" />
                                                                     <ValidationSettings ErrorText=""
                                                                        ValidationGroup="v_cal_prod">
                                                                        <RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                                <%--<dx:ASPxComboBox ID="dd_totalterm" runat="server" DropDownRows="10" AutoPostBack="true" 
                                                                    ValueType="System.String" Width="100px" 
                                                                    OnSelectedIndexChanged="dd_totalterm_SelectedIndexChanged">
                                                                    <ValidationSettings ErrorText=""
                                                                        ValidationGroup="v_cal_prod">
                                                                        <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>--%>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                &nbsp;</td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label70" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Vendor" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                   <dx:ASPxComboBox ID="dd_vendor" runat="server" AutoPostBack="True" 
                                                                        DropDownRows="10" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" 
                                                                        OnTextChanged="dd_vendor_TextChanged" ValueType="System.String" 
                                                                        Width="500px" TabIndex="18" 
                                                                        OnSelectedIndexChanged="dd_vendor_SelectedIndexChanged" >
                                                                       <ValidationSettings ErrorText=""  
                                                                           ValidationGroup="v_cal_prod">
                                                                           <RequiredField ErrorText="" IsRequired="True" />
                                                                           <RegularExpression ErrorText="" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>

<RegularExpression ErrorText=""></RegularExpression>
                                                                       </ValidationSettings>
                                                                    </dx:ASPxComboBox>
                                                              </div>
                                                              <div style="float:left">
                                                                <dx:ASPxTextBox ID="txt_rank_v" runat="server" Width="40px" Enabled="false">
                                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle> 
                                                                </dx:ASPxTextBox> 
                                                              </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label71" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Campaign Type" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_campgType" runat="server" Width="130px" 
                                                                 Height="24px" Text="R" Enabled="false">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                              <%--  <dx:ASPxTextBox ID="txt_totalTerm" runat="server" Width="130px" 
                                                                 Height="24px">   
                                                                </dx:ASPxTextBox>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label72" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Campaign" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                              
                                                               <dx:ASPxComboBox ID="dd_campaign" runat="server" AutoPostBack="True" 
                                                                DropDownRows="10" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" 
                                                                OnSelectedIndexChanged="dd_campaign_SelectedIndexChanged" 
                                                                ValueType="System.String" Width="550px" TabIndex="19">
                                                                   <ValidationSettings ErrorText=""  
                                                                       ValidationGroup="v_cal_prod">
                                                                       <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                   </ValidationSettings>
                                                               </dx:ASPxComboBox>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label73" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Campaign Seq." Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_campSeq" runat="server" Width="130px" ClientInstanceName="txt_campSeq" 
                                                                 Height="24px"  Enabled="False">   
                                                                    <ValidationSettings ValidationGroup="v_cal_prod">
                                                                        <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                    </ValidationSettings>
                                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label74" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Product" Width="120px"></asp:Label>
                                                                 
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left;">
                                                                    <dx:ASPxComboBox ID="dd_product" runat="server" DropDownRows="10" 
                                                                    DropDownStyle="DropDown" IncrementalFilteringMode="Contains" 
                                                                    ValueType="System.String" Width="450px" TabIndex="20" 
                                                                
                                                                    AutoPostBack="true" OnTextChanged="dd_product_TextChanged">
                                                                        <ValidationSettings ErrorText="" 
                                                                            ValidationGroup="v_cal_prod">
                                                                            <RequiredField ErrorText="" IsRequired="True" />
                                                                        </ValidationSettings>
                                                                        <ClientSideEvents TextChanged="function(s, e) {
                                                                            Callback.PerformCallback();LoadingPanel.Show(); }">
                                                                        </ClientSideEvents>

                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                    <dx:ASPxButton ID="btn_vendorScr" runat="server" Text="..." Width="50px" AutoPostBack="true" OnClick="btn_vendorScr_Click">
                                                                        <ClientSideEvents Click="function(s, e) {LoadingPanel.Show(); }"></ClientSideEvents>
                                                                    </dx:ASPxButton>
                                                                </div>
                                                                <div style="float:left;">
                                                                <asp:Label ID="Label8" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small;" ForeColor="red" 
                                                                 Text="(ระบุ 4 ตัวอักษรขึ้นไป)" Width="140px"></asp:Label>
                                                                 </div>
                                                                 <div style="float:left;">
                                                                <asp:Label ID="lb_prodcount" runat="server" ForeColor="Blue" Height="16px" 
                                                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Width="530px"></asp:Label>
                                                                 </div>
                                                               <%-- <dx:ASPxComboBox ID="dd_product_uc" runat="server" Width="250px" 
                                                                 AutoPostBack="True" DropDownRows="10" DropDownStyle="DropDown" 
                                                                 IncrementalFilteringMode="Contains" ValueType="System.String"
                                                                 ></dx:ASPxComboBox>--%>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label75" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Payment ability" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_pay_abl" runat="server" Width="150px" Enabled="false" 
                                                                 Height="24px"  ForeColor="Red">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#f90202" Font-Bold="true" Font-Size="X-Large">
                                                                    </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label76" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Total range" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_total_range" runat="server" Width="100px" Enabled="false" 
                                                                     Height="24px">   
                                                                        <ValidationSettings ErrorText=""  
                                                                            ValidationGroup="v_cal_prod">
                                                                            <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                        </ValidationSettings>
                                                                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <asp:Label ID="Label78" runat="server" 
                                                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                    Text="Non" Width="30px"></asp:Label>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_non" runat="server" Width="50px" 
                                                                     Height="24px" Enabled="False">   
                                                                        <ValidationSettings ErrorText=""  
                                                                            ValidationGroup="v_cal_prod">
                                                                            <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                        </ValidationSettings>
                                                                        <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <asp:Label ID="Label79" runat="server" 
                                                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                    Text="Due" Width="50px"></asp:Label>
                                                                </div>

                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                               &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label77" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Price/item" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_price" runat="server" Width="120px" 
                                                                     Height="24px">   
                                                                        <MaskSettings Mask="&lt;0..9999999g&gt;.&lt;00..99&gt;" 
                                                                            AllowMouseWheel="False" />
<MaskSettings AllowMouseWheel="False" Mask="&lt;0..9999999g&gt;.&lt;00..99&gt;"></MaskSettings>

                                                                        <ValidationSettings ErrorText="" 
                                                                            ValidationGroup="v_cal_prod">
                                                                            <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <asp:Label ID="Label80" runat="server" 
                                                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                    Text="Total Quatity" Width="100px"></asp:Label>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_qty" runat="server" Width="70px"   
                                                                     Height="24px">   
                                                                    
<MaskSettings AllowMouseWheel="False" Mask="099"></MaskSettings>

                                                                        <ValidationSettings ErrorText=""  
                                                                            ValidationGroup="v_cal_prod" ErrorDisplayMode="ImageWithTooltip">
                                                                            <RequiredField ErrorText="" IsRequired="True" />
<RequiredField IsRequired="True" ErrorText=""></RequiredField>
                                                                        </ValidationSettings>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                              </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label81" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Total Purchase" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_purch" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="False">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                 </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label82" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Min price" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                    <%--<asp:Label ID="lb_minPrice" runat="server" style="font-family: Tahoma; font-size: small; color:Gray" Text="" Width="100px"></asp:Label>--%>
                                                                    <dx:ASPxTextBox ID="txt_minPrice" runat="server" Width="100px" Height="24px" ClientInstanceName="txt_minPrice" Enabled="false">   
                                                                        <ValidationSettings ErrorText="" >
                                                                        </ValidationSettings>
                                                                    <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <%--<asp:Label ID="lb_maxPrice" runat="server" style="font-family: Tahoma; font-size: small; color:Gray" Text="" Width="100px"></asp:Label>--%>
                                                                    <dx:ASPxTextBox ID="txt_maxPrice" runat="server" Width="100px" ClientInstanceName="txt_maxPrice" 
                                                                     Height="24px" Enabled="false">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label84" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Loan request" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_loanReq" runat="server" Width="130px" 
                                                                      Height="24px" Enabled="False">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="Red">
                                                                    </DisabledStyle>    
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label83" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Down" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_down" runat="server" Width="100px" 
                                                                     Height="24px">   
                                                                        <MaskSettings Mask="&lt;0..9999999g&gt;.&lt;00..99&gt;" 
                                                                            AllowMouseWheel="False" />
<MaskSettings AllowMouseWheel="False" Mask="&lt;0..9999999g&gt;.&lt;00..99&gt;"></MaskSettings>

                                                                        <ValidationSettings ErrorText="" >
                                                                        </ValidationSettings>
                                                                    
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <asp:Label ID="Label85" runat="server" 
                                                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                    Text="First due amount" Width="120px"></asp:Label>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_fDue_AMT" runat="server" Width="90px" 
                                                                     Height="24px" Enabled="False"> 
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>  
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label86" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="First due date" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_fDue_date" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="False">   
                                                                    <MaskSettings Mask="00/00/0000" />
<MaskSettings Mask="00/00/0000"></MaskSettings>

                                                                <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                </DisabledStyle>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label87" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Duty stamp" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_duty" runat="server" Width="100px" 
                                                                     Height="24px" Enabled="False">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <asp:Label ID="Label88" runat="server" 
                                                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                    Text="Int.Avg %" Width="120px"></asp:Label>
                                                                </div>
                                                                <div style="float:left">&nbsp;&nbsp;</div>
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_Int" runat="server" Width="90px" 
                                                                     Height="24px" Enabled="False">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label89" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Cr.Usg Avg%" Width="130px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                                <dx:ASPxTextBox ID="txt_cru" runat="server" Width="130px" 
                                                                 Height="24px" Enabled="False">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                 </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label90" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Credit bureau" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                    <dx:ASPxTextBox ID="txt_bureau" runat="server" Width="100px" 
                                                                     Height="24px" Enabled="False">
                                                                     <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                    </DisabledStyle>   
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                                <div style="float:left">
                                                                    <asp:Label ID="lb_resNCB" runat="server" 
                                                                    style="font-family: Tahoma; font-size: larger;  font-weight: 700;" 
                                                                    Text="" Width="300px" ForeColor="Blue"></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hd_ncb" />
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                <asp:Label ID="Label92" runat="server" 
                                                                 style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                                 Text="Contract Amount" Width="120px"></asp:Label>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                               <div style="float:left;">
                                                                <dx:ASPxTextBox ID="txt_contractAmt" runat="server" Width="130px" Enabled="false" 
                                                                 Height="24px">
                                                                 <DisabledStyle BackColor="#D3D3D3" ForeColor="#0000FF">
                                                                 </DisabledStyle>   
                                                                </dx:ASPxTextBox>
                                                                </div>
                                                               <div style="float:left;">&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                   
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                   
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                               &nbsp;
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:50%;height:24px;">
                                                                <div style="float:left">
                                                                   &nbsp;
                                                                </div>
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:10%;height:24px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small;width:30%;height:24px;">
                                                               <div style="float:left;">
                                                                 <dx:ASPxButton ID="btn_check_ncb" runat="server" 
                                                                 CausesValidation="true" 
                                                                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                                                 CssPostfix="Office2003Blue" Height="18px" SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                                                 Text="Check ID& NCB" Width="120px" OnClick="btn_check_ncb_Click"  >
                                                                </dx:ASPxButton>
                                                                </div>
                                                               <div style="float:left;">&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                     <dx:ASPxButton ID="btn_cal_TCL" runat="server" Text="Calculate" Width="90px" Height="24px"  
                                                                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                                                                        SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" 
                                                                        onclick="btn_cal_TCL_Click" ValidationGroup="v_cal_prod">
                                                                    </dx:ASPxButton>
                                                                </div>
                                                                <div style="float:left;">&nbsp;&nbsp;</div>
                                                                <div style="float:left;">
                                                                     <dx:ASPxButton ID="btn_keyin" runat="server" Text="Edit" Width="90px" Height="24px" 
                                                                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" CssPostfix="Office2003Blue" 
                                                                        SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" OnClick="btn_keyin_Click">
                                                                    </dx:ASPxButton>
                                                                    <input type="hidden" id="div_position" name="div_position" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                  
                                                    <%--<uc1:UC_ProductSearch id="uc_product" runat="server" />--%>
                                                    
                                                </dx:SplitterContentControl>
                                            </ContentCollection>
                                         </dx:SplitterPane>
                                          <dx:SplitterPane ScrollBars="Vertical" Size="500px"   ShowCollapseBackwardButton="True">
                                        <Separator>
                                            <ButtonStyle><Border BorderColor="Aqua"></Border></ButtonStyle></Separator>
                                            <ContentCollection>
                                                <dx:SplitterContentControl ID="SplitterContentControl5" runat="server" Height="500px" BackColor="#48D1CC"> 
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:250px;height:24px;">
                                                                <asp:Label ID="Label91" runat="server" 
                                                                 style="font-family: Tahoma; font-size: medium; font-weight: 700;" 
                                                                 Text="Description term and rate" Width="250px" Font-Underline="true" ForeColor="Blue"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:GridView ID="gvTerm" runat="server" AutoGenerateColumns="False" 
                                                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" 
                                                            CellPadding="4" EnableModelValidation="True" ForeColor="Black" 
                                                            GridLines="Vertical" Width="99%" 
                                                            AllowPaging="false" 
                                                            EmptyDataText="No records found" 
                                                        OnSelectedIndexChanged="gvTerm_SelectedIndexChanged">
                                                    <emptydatarowstyle backcolor="#D4668D" forecolor="#FFFFFF" Font-Bold="true" HorizontalAlign="center"/>
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F7F7DE" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                          <asp:TemplateField HeaderText="From Term">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_Fterm" runat="server" Text='<%#Eval("from_T")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="To Term">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_Tterm" runat="server" Text='<%#Eval("to_T")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Int%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_intP" runat="server" Text='<%#Eval("int_p")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Int Amount" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_intAMT" runat="server" Text='<%#Eval("int_amt")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                                       <asp:TemplateField HeaderText="INT%(EIR/YEAR)">
            <ItemTemplate>
                <asp:Label ID="lblIntEir" runat="server" Text='<%#Eval("inteir")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
        </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Cru%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_cru_p" runat="server" Text='<%#Eval("cru_p")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Cru Amonth" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_cruAmonth" runat="server" Text='<%#Eval("cru_amt")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                                  <asp:TemplateField HeaderText="CRU%(EIR/YEAR)">
            <ItemTemplate>
                <asp:Label ID="lblCruEir" runat="server" Text='<%#Eval("crueir")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
        </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Init. Free">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_InitFree" runat="server" Text='<%#Eval("int_free")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Total Amonth">
                                                             <ItemTemplate>
                                                                <asp:Label ID="lb_totalAmt" runat="server" Text='<%#Eval("total_amt")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField> 
                                                          <asp:TemplateField HeaderText="Oth">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_OTH" runat="server" Text='<%#Eval("oth")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="PRINCIPAL" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_PRINCIPAL" runat="server" Text='<%#Eval("PRINCIPAL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="INSTALL" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_INSTALL" runat="server" Text='<%#Eval("INSTALL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="DUTY STAMP" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_DUTY_STAMP" runat="server" Text='<%#Eval("DUTY_STAMP")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="INTEREST_BASE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_INTEREST_BASE" runat="server" Text='<%#Eval("INTEREST_BASE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="CR.USAGE BASE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_CR_USAGE" runat="server" Text='<%#Eval("CR_USAGE_BASE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="INIT.FEE BASE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_INIT_FEE_BASE" runat="server" Text='<%#Eval("INIT_FEE_BASE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="CONTRACT" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_CONTRACT" runat="server" Text='<%#Eval("CONTRACT_AMOUNT")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="FIRSTDUE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_FIRSTDUE" runat="server" Text='<%#Eval("FIRST_DATE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="AVG_INTERATE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_AVG_INTERATE" runat="server" Text='<%#Eval("AVG_INTEREST")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="AVG_CREDIT_USAGE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_AVG_CREDIT_USAGE" runat="server" Text='<%#Eval("AVG_CR_USAGE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="CREDIT BUREAU" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_CREDIT_BUREAU" runat="server" Text='<%#Eval("CREDIT_BUREAU")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <%--<asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_PODUTY" runat="server" Text='<%#Eval("PODUTY")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField>
                                                            <ItemTemplate>
                                                           <asp:Label ID="lb_POINTB" runat="server" Text='<%#Eval("POINTB")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_POCRUB" runat="server" Text='<%#Eval("POCRUB")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_POINFB" runat="server" Text='<%#Eval("POINFB")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_POCONA" runat="server" Text='<%#Eval("POCONA")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_POFDAT" runat="server" Text='<%#Eval("POFDAT")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_POAINR" runat="server" Text='<%#Eval("POAINR")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_POACRU" runat="server" Text='<%#Eval("POACRU")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_PODDAT" runat="server" Text='<%#Eval("PODDAT")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>--%>

                                                    </Columns>
                                                 </asp:GridView>
                                                 <table width="100%">
                                                        <tr>
                                                            <td style="text-align: left; font-family: Tahoma; font-size: small; width:250px;height:24px;">
                                                                <asp:Label ID="Label93" runat="server" 
                                                                 style="font-family: Tahoma; font-size: medium; font-weight: 700;" 
                                                                 Text="Description Installment" Width="250px" Font-Underline="true" ForeColor="Blue"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:GridView ID="gv_install" runat="server" AutoGenerateColumns="False" 
                                                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" 
                                                            CellPadding="4" EnableModelValidation="True" ForeColor="Black" 
                                                            GridLines="Vertical" Width="99%" 
                                                            AllowPaging="false" 
                                                            EmptyDataText="No records found" >
                                                    <emptydatarowstyle backcolor="#D4668D" forecolor="#FFFFFF" Font-Bold="true" HorizontalAlign="center" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F7F7DE" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                          <asp:TemplateField HeaderText="Term">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_term" runat="server" Text='<%#Eval("term")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Begin">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_begin" runat="server" Text='<%#Eval("begin")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Principal">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_principal" runat="server" Text='<%#Eval("princ")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Installment">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_Installment" runat="server" Text='<%#Eval("intstallment")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Interest">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_interest" runat="server" Text='<%#Eval("interest")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Cr. Usage">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_CrUsg" runat="server" Text='<%#Eval("cr_use")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Income">
                                                             <ItemTemplate>
                                                                <asp:Label ID="lb_income" runat="server" Text='<%#Eval("income")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField> 
                                                          <asp:TemplateField HeaderText="Cur. Principal">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_CPrincipal" runat="server" Text='<%#Eval("cur_princ")%>'  ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Amonth">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_amonth" runat="server" Text='<%#Eval("amt")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
                                                          </asp:TemplateField>
                                                    </Columns>
                                                 </asp:GridView>
                                                 <table  style="width: 100%" >
                                                    <tr>
                                                        <td style="width: 120px; height: 18px;">
                                                            &nbsp;
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; ">
                                                            &nbsp;
                                                        </td>
                           
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; width:100px;">
                                                           &nbsp;
                               
                                                        </td>
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px;width:250px;">
                                                            &nbsp;
                                                        </td>
                          
                           
                                                        <td style="text-align: left; font-family: Tahoma; font-size: small; height: 18px; ">
                                                                &nbsp;</td>
                                                        <td style="text-align: right; font-family: Tahoma; font-size: small; height: 18px;">
                                                            
                                                        </td>
                                                    </tr>
                                                 </table>
                                                </dx:SplitterContentControl>
                                            </ContentCollection>
                                         </dx:SplitterPane>
                                    </Panes>
                                 </dx:ASPxSplitter> 
                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
            </TabPages>   
        </dx:ASPxPageControl>
                </dx:SplitterContentControl>
            </ContentCollection>
        </dx:SplitterPane>
        </Panes>
    </dx:ASPxSplitter>
    <br />
    <dx:ASPxCallbackPanel ID="ASPxCallback1" runat="server" ClientInstanceName="Callback" >
        <ClientSideEvents EndCallback="function(s, e) { LoadingPanel.Hide(); }" />
<ClientSideEvents EndCallback="function(s, e) { LoadingPanel.Hide(); }"></ClientSideEvents>
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True"></dx:PanelContent>
            </PanelCollection>
    </dx:ASPxCallbackPanel>
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="LoadingPanel" runat="server" Width="250px" Height="150px" 
    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" >
    </dx:ASPxLoadingPanel>

    <dx:ASPxPopupControl ID="PopupMsg" ClientInstanceName="PopupMsg"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Popup Msg" CloseAction="CloseButton" AllowDragging="True" 
        AutoUpdatePosition="True" Modal="True" Width="450px" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">                        
            <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            </ContentStyle>
            <HeaderStyle>
            <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" 
                PaddingTop="3px" />
<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblMsgTH" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblMsgEN" runat="server" Font-Names="Tahoma" Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblAppNo" runat="server" Font-Names="Tahoma" Font-Size="Medium" Font-Bold="true" Style="color: #006633"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblContract" runat="server" Font-Names="Tahoma" Font-Size="Medium" Font-Bold="true" Style="color: blue"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="center" >
                                <dx:ASPxButton ID="btnClosePopupMsg" runat="server" Text="OK" Width="100px" >
                                    <ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }" />
<ClientSideEvents Click="function(s, e) { PopupMsg.Hide(); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

       

   <dx:ASPxPopupControl ID="PopupConfirmSave" ClientInstanceName="PopupConfirmSave"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Confirm Save" CloseAction="CloseButton" AllowDragging="True" 
        AutoUpdatePosition="True" Modal="True" Width="450px" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">                        
            <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            </ContentStyle>
            <HeaderStyle>
            <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" 
                PaddingTop="3px" />
<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" 
                    SupportsDisabledAttribute="True">
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblCreditBureau" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>  
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblCreditReview" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>   
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmMsgTH" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmMsgEN" runat="server" ClientInstanceName="lblConfirmMsgEN" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>                                             
                    </table>
                    <br />
                    <asp:HiddenField ID="hid_Confirm" runat="server" />
                    <br />
                    
                    <asp:HiddenField ID="hid_App" runat="server" />
                    <asp:HiddenField ID="hid_cont" runat="server" />
                    <asp:HiddenField ID="hid_CRMD" runat="server" />
                    <asp:HiddenField ID="hid_CRRW" runat="server" />
                    <asp:HiddenField ID="hid_RejectEN" runat="server" /> 
                    <asp:HiddenField ID="hid_RejectTH" runat="server" />
                    <asp:HiddenField ID="hid_RejectCK" runat="server" />
                    <asp:HiddenField ID="hid_bureau" runat="server" />
                       <asp:HiddenField ID="hid_inteirsum" runat="server" />
                    <asp:HiddenField ID="hid_crueirsum" runat="server" />
                    <asp:HiddenField ID="hid_installment" runat="server" /> 
                    <asp:HiddenField ID="hid_lastinstallment" runat="server" /> 
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="right" width="50%" >
                                <dx:ASPxButton ID="btnConfirmSave" runat="server" Text="OK" Width="100px" 
                                    OnClick="btnConfirmSave_Click" >  
<ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();LoadingPanel.Show(); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td align="left" width="50%" >
                                <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" 
                                    OnClick="btnConfirmCancel_Click">
                                    <%--<ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();LoadingPanel.Show();}" />--%>
<ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();LoadingPanel.Show();}"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


        <dx:ASPxPopupControl ID="Popup_Commerce" ClientInstanceName="Popup_Commerce"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Comfirm commercial register" CloseAction="CloseButton" AllowDragging="True" 
        AutoUpdatePosition="True" Modal="True" Width="450px" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">                        
            <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            </ContentStyle>
            <HeaderStyle>
            <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" 
                PaddingTop="3px" />
<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" 
                    SupportsDisabledAttribute="True">
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lbl_c_msg_en" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
              
                    <br />
                    
                    
                    
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="right" width="50%" >
                                <dx:ASPxButton ID="btn_c_ok" runat="server" Text="OK" Width="100px">  
                                    <ClientSideEvents Click="function(s, e) { Popup_Commerce.Hide();LoadingPanel.Show(); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td align="left" width="50%" >
                                <dx:ASPxButton ID="btn_c_cancel" runat="server" Text="Cancel" Width="100px" 
                                    OnClick="btn_c_cancel_Click" >
                                    <ClientSideEvents Click="function(s, e) { Popup_Commerce.Hide(); }" />
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
         <!--  Product Popup -->
        <dx:ASPxPopupControl ID="Popup_ScrProd" ClientInstanceName="Popup_ScrProd" ShowPageScrollbarWhenModal="true"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Search Product" CloseAction="CloseButton" AllowDragging="True" 
        AutoUpdatePosition="false" Modal="True" Width="1300px" Height="100%" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">                        
            <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            </ContentStyle>
            <HeaderStyle>
            <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" 
                PaddingTop="3px" />
<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
                   
                    <table width="100%" align="left">
                        <tr>
                            <td align="left" class="style2" style="text-align: right; width: 120px;">
                                <asp:Label ID="Label114" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Product Type" Width="100px"></asp:Label>
                                    &nbsp;&nbsp;
                                
                            </td>
                            <td class="style6" style="text-align: left;">
                                <div style="float:left;">
                                    <dx:ASPxComboBox runat="server" Width="250px" ID="dd_prodType" DropDownRows="30"></dx:ASPxComboBox>
                                    <%--<dx:ASPxTextBox runat="server" Width="150px" ID="txt_prodType"></dx:ASPxTextBox>  --%>     
                                    
                                </div>
                                <div style="float:left;">
                                </div>
                             
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style2" style="text-align: right; width: 120px;">
                                <asp:Label ID="Label115" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Brand" Width="110px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <div style="float:left">
                                    <dx:ASPxComboBox runat="server" Width="250px" ID="dd_prodBrand" 
                                        OnSelectedIndexChanged="dd_prodBrand_SelectedIndexChanged" 
                                        OnTextChanged="dd_prodBrand_TextChanged" autopostback="True" DropDownRows="30" 
                                dropdownstyle="DropDown" incrementalfilteringmode="Contains"  
                                backcolor="#FFFFC4" valuetype="System.String" >
                                    </dx:ASPxComboBox>
                                   <%-- <dx:ASPxTextBox runat="server" Width="150px" ID="txt_prodBrand"></dx:ASPxTextBox> --%>
                                </div>
                                <div style="float:left;">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="true" ForeColor="red" 
                                        Text="ระบุชื่อ Brand และกด [ENTER]">
                                    </asp:Label>
                                </div>
                             </td>
                        </tr>
                        <tr>
                            <td align="left" class="style2" style="text-align: right; width: 120px;">
                                <asp:Label ID="Label7" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Product code" Width="110px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <div style="float:left">
                                    <dx:ASPxComboBox runat="server" Width="250px" ID="dd_prodcode" 
                                        OnSelectedIndexChanged="dd_prodcode_SelectedIndexChanged" 
                                        OnTextChanged="dd_prodcode_TextChanged" autopostback="True" DropDownRows="30" 
                                        dropdownstyle="DropDown" incrementalfilteringmode="Contains"  
                                        backcolor="#FFFFC4" valuetype="System.String" >
                                    </dx:ASPxComboBox>
                                    <div style="float:left;">
                                </div>
                                    <%--<dx:ASPxTextBox runat="server" Width="150px" ID="txt_prodcode"></dx:ASPxTextBox>--%>
                                </div>
                             
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" 
                                    Text="ระบุ Product code และกด [ENTER]"></asp:Label>
                             
                            </td>
                        </tr>
                        
                        <tr>         
                           <td align="left" class="style2" style="text-align: right; width: 120px;">
                               &nbsp;</td>                   
                            <td class="style6" style="text-align: left;">
                                <div style="float:left;">
                                </div>
                                <div style="float:left">&nbsp;&nbsp;</div> 
                                <div style="float:left;">
                                    <dx:ASPxButton ID="btn_src_pdItem" runat="server" Text="Search" Width="100px" 
                                        OnClick="btn_src_pdItem_Click" CausesValidation="true">
                                        <ClientSideEvents Click="function(s, e) {LoadingPanel.Show(); }"></ClientSideEvents>
                                    </dx:ASPxButton>
                                </div>
                                <div style="float:left">&nbsp;&nbsp;</div>
                                <div style="float:left">
                                    <dx:ASPxButton ID="btn_cancel_prod" runat="server" Text="Cancel" Width="100px" 
                                       OnClick="btn_cancel_prod_Click" >

                                    </dx:ASPxButton>
                                </div>
                                <asp:Label ID="lb_res_search" runat="server"  
                                    style="font-family: Tahoma; font-size: small; font-weight: 700; color:red;" 
                                    Width="300px"></asp:Label>
                            </td>                            
                        </tr>
                        
                            <tr>
                                <td colspan="2" align="left">
                                <div style="overflow-y: scroll; height: 300px;" >
                              <asp:GridView ID="gv_item" runat="server" AutoGenerateColumns="False" 
                               BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" 
                               CellPadding="4" EnableModelValidation="True" ForeColor="Black" 
                               GridLines="Vertical" Width="100%" 
                                EmptyDataText="No records found" OnRowCommand="gv_item_RowCommand" >
                                                    <emptydatarowstyle backcolor="#D4668D" forecolor="#FFFFFF" Font-Bold="true" HorizontalAlign="center"/>
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F7F7DE" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                           <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                                                    CommandName="SEL" Text="SEL" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Campaign">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Campaign" runat="server" Text='<%#Eval("C07CMP")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Camp Seq">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Campaign_sql" runat="server" Text='<%#Eval("C07CSQ")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Price" runat="server" Text='<%#Eval("C07PRC")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Down">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Down" runat="server" Text='<%#Eval("C07DOW")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="item code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_code" runat="server" Text='<%#Eval("T44ITM")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Desc" runat="server" Text='<%#Eval("T44DES")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" Wrap="false" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="150px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Type" runat="server" Text='<%#Eval("T44TYP")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Brand">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Brand" runat="server" Text='<%#Eval("T44BRD")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Code44" runat="server" Text='<%#Eval("T44COD")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Model">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Model" runat="server" Text='<%#Eval("T44MDL")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Group">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_Group" runat="server" Text='<%#Eval("T44PGP")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="MIN Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_MIN" runat="server" Text='<%#Eval("C07MIN")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="MAX Price">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_item_MAX" runat="server" Text='<%#Eval("C07MAX")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                    </Columns>
                                                 </asp:GridView>
                                                 </div>
                                        </td>
                                        </tr>
                       </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <!--  Note Popup -->
        <dx:ASPxPopupControl ID="Popup_AddNote" ClientInstanceName="Popup_AddNote" ShowPageScrollbarWhenModal="true"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Add Note" CloseAction="CloseButton" AllowDragging="True" 
        AutoUpdatePosition="True" Modal="True" Width="900px" Height="500px" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        SpriteCssFilePath="~/App_Themes/BlackGlass/{0}/sprite.css">                        
            <LoadingPanelImage Url="~/App_Themes/BlackGlass/Web/Loading.gif">
            </LoadingPanelImage>
            <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            </ContentStyle>
            <HeaderStyle>
            <Paddings PaddingBottom="6px" PaddingLeft="15px" PaddingRight="6px" 
                PaddingTop="3px" />
<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                   
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style2" style="text-align: right; width: 100px;">
                                <asp:Label ID="Label4" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Action code:" Width="100px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <dx:ASPxComboBox ID="dd_ActionCode" runat="server" autopostback="True" dropdownrows="15" 
                                dropdownstyle="DropDown" incrementalfilteringmode="Contains"  
                                backcolor="#FFFFC4" valuetype="System.String" width="300px">
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style2" style="text-align: right; width: 100px;">
                                <asp:Label ID="Label6" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Result code:" Width="100px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                 <dx:ASPxComboBox ID="dd_ResCode" runat="server" autopostback="True" dropdownrows="15" 
                                dropdownstyle="DropDown" incrementalfilteringmode="Contains"  
                                backcolor="#FFFFC4" valuetype="System.String" width="300px">
                                </dx:ASPxComboBox>
                            </td>
                          
                        </tr>
                        <tr>
                            <td align="right" class="style2" style="text-align: right; width: 100px;">
                                &nbsp;
                            </td>
                            <td class="style6" style="text-align: left;">
                               <dx:ASPxMemo ID="txt_memoReason" runat="server" Width="500px" Height="80px">
                               </dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>         
                           <td align="left">
                                &nbsp;
                           </td>                   
                            <td align="left">
                                <div style="float:left">
                                    <dx:ASPxButton ID="btn_saveNote" runat="server" Text="OK" Width="100px" AutoPostBack="true" 
                                         OnClick="btn_saveNote_Click" >
                                        <ClientSideEvents Click="function(s, e) {LoadingPanel.Show(); }"></ClientSideEvents>
                                    </dx:ASPxButton>
                                </div>
                                <div style="float:left">&nbsp;&nbsp;</div>
                                <div style="float:left">
                                    <dx:ASPxButton ID="btn_cancel_Save" runat="server" Text="Cancel" Width="100px" AutoPostBack="true" 
                                        CausesValidation="false" OnClick="btn_cancel_Save_Click">

                                    </dx:ASPxButton>
                                </div>
                                <div style="float:left">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="red" Font-Bold="true" Width="200px"></asp:Label>
                                </div>
                                
                            </td>                            
                        </tr>
                        <tr>                            
                            <td align="center" colspan="2" >
                              <asp:GridView ID="gvNote" runat="server" AutoGenerateColumns="False" 
                               BackColor="White" BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" 
                               CellPadding="4" EnableModelValidation="True" ForeColor="Black" 
                               GridLines="Vertical" Width="100%" AllowPaging="True" 
                                EmptyDataText="No records found" 
                                    OnPageIndexChanging="gvNote_PageIndexChanging">
                                                    <emptydatarowstyle backcolor="#D4668D" forecolor="#FFFFFF" Font-Bold="true" HorizontalAlign="center"/>
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#87CEFA" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F7F7DE" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                          <asp:TemplateField HeaderText="Note date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_M38DAT" runat="server" Text='<%#Eval("M38DAT_")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                             <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Note time">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_M38TIM" runat="server" Text='<%#Eval("M38TIM")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Note By">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_M38USR" runat="server" Text='<%#Eval("M38USR")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Action code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_M38ACD" runat="server" Text='<%#Eval("M38ACD")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Result code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_M38RCD" runat="server" Text='<%#Eval("M38RCD")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px"/>
                                                          </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lb_M38DES" runat="server" Text='<%#Eval("M38DES")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="600px"/>
                                                          </asp:TemplateField>
                                                    </Columns>
                                                 </asp:GridView>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    
    <!-- Popup Note Cancel -->
    <dx:ASPxPopupControl ID="P_note_cancel" 
    runat="server" Height="146px" Width="600px" AllowDragging="True" 
    AutoUpdatePosition="True" ClientInstanceName="P_note_TCL" ShowPageScrollbarWhenModal="true"
    CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" Font-Bold="False" 
    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" HeaderText = "Confirm Save">
                                              <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" SupportsDisabledAttribute="True">
    <table align="center" width="100%">
        <tr>
            <td align="center" style="text-align: left">
                <asp:Label ID="Label206" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Action Code" Width="80px"></asp:Label>
            </td>
            <td align="left">
                <div style="float:left">
                    <dx:ASPxComboBox ID="D_action" runat="server" BackColor="#FFFFC4" 
                        ValueType="System.String" Width="300px" incrementalfilteringmode="StartsWith"  >
                    </dx:ASPxComboBox>
                </div>
                <%--<div style="float:right">
                    <dx:ASPxButton ID="btn_refresh" runat="server" RenderMode="Link" Text="Refresh code"
                    Width="120px" Height="15px" AutoPostBack="true" ImagePosition="Right" 
                        Image-Url="~/Images/refresh.png" Image-Height="15" Image-Width="15" 
                        OnClick="btnLinkImageAndText_Click">
                        <Image Height="15px" Url="~/Images/refresh.png" Width="15px">
                        </Image>
                    </dx:ASPxButton>
                </div>--%>
                
            </td>
        </tr>
        <tr>
            <td align="center" style="text-align: left">
                <asp:Label ID="Label207" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Reason Code" Width="100px"></asp:Label>
            </td>
            <td align="left">
                <dx:ASPxComboBox ID="D_reason" runat="server" AutoPostBack="True" 
                    BackColor="#FFFFC4" DropDownRows="12" DropDownStyle="DropDown" 
                    IncrementalFilteringMode="StartsWith" ValueType="System.String" Width="300px">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" style="text-align: left">
                <dx:ASPxMemo ID="E_note_Cancel" runat="server" Height="51px" Width="100%" 
                    Rows="5" ></dx:ASPxMemo>
                <%--<asp:TextBox ID="E_note_TCL" runat="server" Height="51px" Width="100%"></asp:TextBox>--%>
            </td>
        </tr>
    </table>
    <asp:Label ID="L_msg_note_Cancel" runat="server" Font-Names="Tahoma" 
        Font-Size="Small" ForeColor="Red"></asp:Label>
       
    <br />
    <table align="center" width="100%">
        <tr>
            <td align="right" style="text-align: center" width="50%">
                <dx:ASPxButton ID="B_savenote_Cancel" runat="server" 
                    Text="Save" Width="100px" OnClick="B_savenote_Cancel_Click">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel.Show();
}" />
                </dx:ASPxButton>
            </td>
            <td align="left" style="text-align: center" width="50%">
                <dx:ASPxButton ID="B_cancelnote" runat="server" 
                    Text="Cancel" Width="100px" OnClick="B_cancelnote_Click">
                    <ClientSideEvents Click="function(s, e) {
    LoadingPanel.Show();
}" />
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td align="right" style="text-align: center" width="50%">
               &nbsp;
            </td>
            <td align="left" style="text-align: center" width="50%">
                 
            </td>
        </tr>

    </table>
                                                  </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>


    <!-----  ---------------->
    
                
                
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
                
            </td>
        </table>
</asp:Content>

