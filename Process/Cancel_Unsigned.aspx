<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="Cancel_Unsigned.aspx.cs" Inherits="ManageData_WorkProcess_Cancel_Unsigned" %>
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
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxLoadingPanel" tagprefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phContents" Runat="Server">
<script type="text/javascript" src='<%=ResolveUrl("~/Js/shortcut.js")%>' ></script>
<script  type="text/javascript"  src='<%=ResolveUrl("~/Js/ProtectJs.js")%>' ></script>

    <table style="width: 100%">
        <tr>
            <td style="text-align: left; width: 100%; height: 100%;">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    GroupBoxCaptionOffsetY="-28px" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100%" 
                    Height="136px">
                    <ContentPaddings Padding="5px" />
                   
<ContentPaddings Padding="5px"></ContentPaddings>
                   
                    <HeaderTemplate>
                        Cancel Unsign
                    </HeaderTemplate>
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label1" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Search by" Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                <div style="float:left;">
                    <asp:DropDownList ID="dd_search" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="dd_search_SelectedIndexChanged">
                        <asp:ListItem Text="Application No." Value="1"></asp:ListItem>
                        <asp:ListItem Text="Contract No." Value="2" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="float:left;">&nbsp;</div>
                <div style="float:left;">
                    <dx:ASPxTextBox ID="txt_search" runat="server" Width="150px" >
                        
                        <MaskSettings Mask="0000-000-000000000" />
                        
                     </dx:ASPxTextBox>
                 </div>
                 <div style="float:left;">
                     <asp:Button ID="btn_search" runat="server" Text="Search" Width="120px" 
                         BackColor="#87CEFA" BorderColor="#87CEFA" ForeColor="#0040FF" 
                         BorderStyle="Outset" OnClick="btn_search_Click" >
                     </asp:Button> 
                 </div>            
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
               <div style="float:left">
                <asp:Label ID="lb_err" runat="server" style="font-family: Tahoma; font-size: medium;" Width="150px" ForeColor="Red"></asp:Label>
                </div>
                <div style="float:left">
                                <asp:Label ID="lblMsg" runat="server"  ForeColor="red" Font-Size="Medium" Font-Bold="true" Width="320px"></asp:Label>
                            </div>  
            </td>
        </tr>
    </table>
    <br /> 
<table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px; background-color:#87CEFA" colspan="4" >
                <asp:Label ID="lb_Detail" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Blue" Text="Customer Detail"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label6" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Name" Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                <asp:Label ID="lb_name" runat="server" style="font-family: Tahoma; font-size: small;" Text="" Width="150px"></asp:Label>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
               <asp:Label ID="Label17" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Sex" Width="150px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                <asp:Label ID="lb_sex" runat="server" style="font-family: Tahoma; font-size: small;" Text="" Width="150px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label3" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Application No" Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                <asp:Label ID="lb_appNo" runat="server" style="font-family: Tahoma; font-size: small;" Text="" Width="150px"></asp:Label>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
               <asp:Label ID="Label5" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="ID No." Width="150px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                <asp:Label ID="lb_ID" runat="server" style="font-family: Tahoma; font-size: small;" Text="" Width="150px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label8" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="EBC No." Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                 <asp:Label ID="lb_ebc" runat="server" style="font-family: Tahoma; font-size: small;" Text="" Width="150px"></asp:Label>          
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
               <asp:Label ID="Label9" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="CIS No" Width="150px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                 <asp:Label ID="lb_csn" runat="server" style="font-family: Tahoma; font-size: small;" 
                    Text="" Width="150px"></asp:Label>        
             </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label10" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Contract No" Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;" >
                <asp:Label ID="lb_cont" runat="server" style="font-family: Tahoma; font-size: small; color:Blue; font-weight:bold; " 
                    Text="" Width="150px"></asp:Label>  
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
               <asp:Label ID="Label2" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Loan Amount" Width="150px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
               <asp:Label ID="lb_loan_amt" runat="server" style="font-family: Tahoma; font-size: small;" 
                    Text="" Width="150px"></asp:Label>          
             </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label13" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="Product" 
                    Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;" >
                 <asp:Label ID="lb_prod_code" runat="server" 
                     style="font-family: Tahoma; font-size: small;" Width="50px"></asp:Label>
                 <asp:Label ID="lb_prod" runat="server" 
                     style="font-family: Tahoma; font-size: small; margin-bottom: 8px;" 
                     Width="350px"></asp:Label>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
                <asp:Label ID="Label4" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="QTY" 
                    Width="150px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                   &nbsp;
                     <asp:Label ID="lb_qty" runat="server" 
                       style="font-family: Tahoma; font-size: small;" Width="150px"></asp:Label>
             </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label12" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Contract Amount" Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;" >
                 <asp:Label ID="lb_contract_AM" runat="server" 
                     style="font-family: Tahoma; font-size: small;" Width="150px"></asp:Label>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
               &nbsp;
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                 &nbsp;       
             </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;">
                <asp:Label ID="Label7" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Credit limit" Width="150px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;" >
                 <asp:Label ID="lb_credit" runat="server" style="font-family: Tahoma; font-size: small;" 
                    Text="" Width="150px"></asp:Label>          
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px;" >
               <asp:Label ID="Label14" runat="server" style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Limit Available" Width="150px"></asp:Label>
            </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                   <asp:Label ID="lb_lmA" runat="server" style="font-family: Tahoma; font-size: small;" 
                    Text="" Width="100px"></asp:Label>          
             </td>
        </tr>
    </table>
    <br />
    <table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:150px;height:24px; background-color:#87CEFA" colspan="4" >
                <asp:Label ID="Label11" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Blue" Text="Add note"></asp:Label>
            </td>
        </tr>
        </table>
        <asp:Panel ID="PN_AddNote" runat="server">
        <table width="100%" align="center">
                    <tr>
                        <td align="right" class="style2" style="text-align: right; width: 100px;">
                            <asp:Label ID="Label23" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Action code:" Width="100px"></asp:Label>
                        </td>
                        <td class="style6" style="text-align: left;">
                            <div style="float:left">
                                <dx:ASPxComboBox ID="dd_ActionCode" runat="server"  dropdownrows="20" 
                                dropdownstyle="DropDown" incrementalfilteringmode="StartsWith"  
                                backcolor="#FFFFC4" valuetype="System.String" width="300px">
                                </dx:ASPxComboBox>
                            </div>
                            <div style="float:right">
                                     <dx:ASPxButton ID="btn_refresh" runat="server" RenderMode="Link" Text="Refresh code"
                                        Width="120px" Height="15px" AutoPostBack="true" ImagePosition="Right" 
                                         Image-Url="~/Images/refresh.png" Image-Height="15" Image-Width="15" 
                                         OnClick="btnLinkImageAndText_Click">
                                         
                                         <Image Height="15px" Url="~/Images/refresh.png" Width="15px">
                                         </Image>
                                         
                                     </dx:ASPxButton>
                                </div>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style2" style="text-align: right; width: 100px;">
                            <asp:Label ID="Label24" runat="server" 
                                style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Text="Result code:" Width="100px"></asp:Label>
                        </td>
                        <td class="style6" style="text-align: left;">
                            <div style="float:left">
                                <dx:ASPxComboBox ID="dd_ResCode" runat="server"  dropdownrows="10" 
                                dropdownstyle="DropDown" incrementalfilteringmode="StartsWith"   
                                backcolor="#FFFFC4" valuetype="System.String" width="300px"  >  
                                </dx:ASPxComboBox>
                            </div>
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
                                <dx:ASPxButton ID="btn_save" runat="server" Text="Cancel Unsign" Width="100px" 
                                    OnClick="btn_save_Click"  >
                                    <ClientSideEvents Click="function(s, e) {LoadingPanel_Unsinged.Show();}" />
                                </dx:ASPxButton>
                            </div>
                            <div style="float:left">&nbsp;&nbsp;</div>
                            <div style="float:left">
                                <dx:ASPxButton ID="btn_cancel" runat="server" Text="Clear" Width="100px" 
                                    OnClick="btn_cancel_Click" >
                                  
                                </dx:ASPxButton>
                            </div>
                            <div style="float:left">&nbsp;&nbsp;</div>
                            <%--<div style="float:left">
                                <asp:Label ID="lblMsg" runat="server"  ForeColor="red" Font-Size="Medium" Font-Bold="true" Width="500px"></asp:Label>
                            </div>  --%>  
                        </td>                            
                    </tr>
                  </table>

                  </asp:Panel>
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
<Paddings PaddingLeft="15px" PaddingTop="3px" PaddingRight="6px" PaddingBottom="6px"></Paddings>
            </HeaderStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" 
                    SupportsDisabledAttribute="True">
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmMsgTH" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #CC0000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmMsgEn" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <dx:ASPxLabel ID="lblConfirmSal" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Large" Font-Bold="true" Style="color: #0000FF"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                  
                    <br />
                    
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="right" width="50%" >
                                <dx:ASPxButton ID="btnConfirmSave" runat="server" Text="OK" Width="100px" 
                                    OnClick="btnConfirmSave_Click" >
<ClientSideEvents Click="function(s, e) { LoadingPanel_Unsinged.Show();PopupConfirmSave.Hide(); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td align="left" width="50%" >
                                <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" 
                                    OnClick="btnConfirmCancel_Click" >
                                    <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
<ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();}"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>                            
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


                                                       
                        <br />
      <dx1:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
          ClientInstanceName="LoadingPanel_Unsinged" Height="150px" Width="250px">
      </dx1:ASPxLoadingPanel>
    

   
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>

            </td>
            </tr>
        </table>
</asp:Content>

