<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="Update_AutoPay.aspx.cs" Inherits="ManageData_WorkProcess_Update_AutoPay" %>
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
                        Update payment type of auto pay
                    </HeaderTemplate>
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label1" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Select type" Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                 <asp:RadioButtonList ID="rb_type" runat="server" RepeatDirection="Horizontal" 
                     Width="500px" Font-Bold="true" BackColor="#99FFFF" AutoPostBack="true" 
                     OnSelectedIndexChanged="rb_type_SelectedIndexChanged">
                    <asp:ListItem Value="1" Text="Send to bank" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Ready to auto pay"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Change payment type"></asp:ListItem>
                 </asp:RadioButtonList>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:500px;height:24px;" >
                <div style="float:right">
                    <dx:ASPxButton ID="btn_refresh" runat="server" RenderMode="Link" Text="Refresh"
                    Width="130px" Height="15px" AutoPostBack="true" ImagePosition="Right" 
                        Image-Url="~/Images/refresh.png" Image-Height="15" Image-Width="15" 
                        OnClick="btnLinkImageAndText_Click">
                                         
                        <Image Height="15px" Url="~/Images/refresh.png" Width="15px">
                        </Image>
                                         
                    </dx:ASPxButton>
               </div>
                <dx:ASPxButton ID="btn_addnote" runat="server" 
                    CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                    CssPostfix="Office2003Blue" Height="24px" OnClick="btn_addnote_Click" 
                    SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css" Text="Add Note" 
                    Width="88px">
                    <ClientSideEvents Click="function(s, e) {LoadingPanel.Show();}" />
                </dx:ASPxButton>
                &nbsp;</td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label3" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="ID No." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                <dx:ASPxTextBox runat="server" Width="150px" ID="txt_id"   DisabledStyle-BackColor="Silver">
                    <MaskSettings Mask="9999999999999" />
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxTextBox>
                <%--<asp:TextBox runat="server" 
                ></asp:TextBox>--%>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label5" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="App No." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                <dx:ASPxTextBox runat="server" Width="150px" ID="txt_app"   DisabledStyle-BackColor="Silver">
                    <MaskSettings Mask="99999999999" />
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxTextBox>
                <%--<asp:TextBox runat="server" Width="150px" ID="txt_app"></asp:TextBox>--%>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label6" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="EBC No." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                <dx:ASPxTextBox runat="server" Width="150px" ID="txt_ebc"   DisabledStyle-BackColor="Silver">
                    <MaskSettings Mask="9999999999999999" />
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxTextBox>
                <%--<asp:TextBox runat="server" Width="150px" ID="txt_ebc"></asp:TextBox>--%>
             </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label7" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="CIS No." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                <dx:ASPxTextBox runat="server" Width="150px" ID="txt_csn"  DisabledStyle-BackColor="Silver">
                    <MaskSettings Mask="999999999" />
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxTextBox>
                <%--<asp:TextBox runat="server" Width="150px" ID="txt_csn"></asp:TextBox>--%>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label8" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Contract No." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                <dx:ASPxTextBox runat="server" Width="150px" ID="txt_cont" DisabledStyle-BackColor="Silver">
                    <MaskSettings Mask="9999999999999999" />
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxTextBox>
               <%-- <asp:TextBox runat="server" Width="150px" ID="txt_cont"></asp:TextBox>--%>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label9" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Bank Code." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                <dx:ASPxComboBox ID="dd_bankCode" runat="server" Width="150px" DisabledStyle-BackColor="Silver">
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxComboBox>    
             </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label10" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Branch." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;" >
                 <dx:ASPxComboBox ID="dd_brn" runat="server" ValueType="System.String" 
                     Width="150px" DisabledStyle-BackColor="Silver">
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxComboBox>
             </td>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label11" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Date." Width="100px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:400px;height:24px;" colspan="2" >
                <asp:TextBox runat="server" Width="100px" ID="txt_from"></asp:TextBox> 
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                Format="dd/MM/eeee" PopupButtonID="ImageButton1" 
                                TargetControlID="txt_from">
                </cc1:CalendarExtender>
                <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/CalendarBtn.gif" />&nbsp;&nbsp;
                <asp:TextBox runat="server" Width="100px" ID="txt_to"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" 
                                Format="dd/MM/eeee" PopupButtonID="ImageButton2" 
                                TargetControlID="txt_to">
                </cc1:CalendarExtender>
                <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="Bottom" ImageUrl="~/Images/CalendarBtn.gif" />
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                <div style="float:left">
                
                <dx:ASPxButton  ID="btn_search" runat="server" Height="24px"
                 SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                 CssPostfix="Office2003Blue"
                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                 Text="Search" Width="70px" OnClick="btn_search_Click">
                 <ClientSideEvents Click="function(s, e) {LoadingPanel.Show();}" />
                </dx:ASPxButton>
                 <input runat="server" ID="hd_idNo" type="hidden"></input>
</input>
                     </input>
                <input id="hd_csn" runat="server" type="hidden"></input>
                    </input>
                    </input>
                </div>
                <div style="float:left">&nbsp;&nbsp;</div>
                 <dx:ASPxButton  ID="btn_clear" runat="server"
                 SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                 CssPostfix="Office2003Blue"
                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                 Height="24px" Text="Clear" Width="70px">
                </dx:ASPxButton>

                <%-- <asp:Button ID="btn_search" runat="server" BackColor="#66CCFF" Height="24px" 
                     Text="Search" Width="70px" OnClick="btn_search_Click" />&nbsp;&nbsp;
                 <asp:Button ID="btn_clear" runat="server" BackColor="#66CCFF" Height="24px" Text="Clear" Width="70px" />--%>
                
             </td>
        </tr>
    </table>
    
    <asp:Panel ID="pn_cust" runat="server" Height="150px" ScrollBars="Vertical">
        <asp:GridView ID="gv_cust" runat="server" AutoGenerateColumns="False" 
            BackColor="White" PageSize="15" 
        BorderColor="#DEDFDE" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
        EnableModelValidation="True" ForeColor="Black"  GridLines="Vertical" 
            Width="99%" AllowPaging="false" 
        EmptyDataText="No records found" OnRowCommand="gv_cust_RowCommand" >
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
                    <asp:LinkButton ID="LinkButton_Sel" runat="server" CausesValidation="False" 
                            CommandName="Sel" Text="Sel" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:Label ID="lb_Name" runat="server" Text='<%#Eval("fName")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign = "Left" VerticalAlign="Middle" />
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"  />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID Card">
                <ItemTemplate>
                    <asp:Label ID="lb_ID" runat="server" Text='<%#Eval("m00idn")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="App No">
                <ItemTemplate>
                    <asp:Label ID="lb_AppNo" runat="server" Text='<%#Eval("p1apno")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
            </asp:TemplateField>   
            <asp:TemplateField HeaderText="Loan AVP">
                <ItemTemplate>
                    <asp:Label ID="lb_Loan" runat="server" Text='<%#Eval("p1pram")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign = "Right" VerticalAlign="Middle" />
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
            </asp:TemplateField>  
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lb_p1pram" runat="server" Text='<%#Eval("p1pram")%>' ></asp:Label>
                    <asp:Label ID="lb_p1csno" runat="server" Text='<%#Eval("p1csno")%>' ></asp:Label>
                    <asp:Label ID="lb_p1vdid" runat="server" Text='<%#Eval("p1vdid")%>' ></asp:Label>
                    <asp:Label ID="lb_p10nam" runat="server" Text='<%#Eval("p10nam")%>' ></asp:Label>
                    <asp:Label ID="lb_p1payt" runat="server" Text='<%#Eval("p1payt")%>' ></asp:Label>
                    <asp:Label ID="lb_p1pbcd" runat="server" Text='<%#Eval("p1pbcd")%>' ></asp:Label>
                    <asp:Label ID="lb_p1pbrn" runat="server" Text='<%#Eval("p1pbrn")%>' ></asp:Label>
                    <asp:Label ID="lb_p1paty" runat="server" Text='<%#Eval("p1paty")%>' ></asp:Label>
                    <asp:Label ID="lb_p1pano" runat="server" Text='<%#Eval("p1pano")%>' ></asp:Label>
                    <asp:Label ID="lb_p1cont" runat="server" Text='<%#Eval("p1cont")%>' ></asp:Label>
                    <asp:Label ID="lb_p1brn" runat="server" Text='<%#Eval("p1brn")%>' ></asp:Label>
                    <asp:Label ID="lb_p00bst" runat="server" Text='<%#Eval("p00bst")%>' ></asp:Label>
                    <asp:Label ID="lb_p00doc" runat="server" Text='<%#Eval("p00doc")%>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign = "Center" VerticalAlign="Middle" />
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top"/>
            </asp:TemplateField>                                               
    </Columns>
    </asp:GridView>
</asp:Panel>

<br/>
<table width="100%">
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:550px;height:24px;background-color:#99FFFF; " colspan="6">
                <asp:Label ID="lb_oper" runat="server" 
                    style="font-family: Tahoma; font-size: medium; font-weight: 700; color:Blue; " Text="" Width="200px"></asp:Label>
                <asp:Label ID="lb_name_sh" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" Width="250px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label2" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Vendor ID" Width="100px" Height="16px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:480px;height:24px;" colspan="3">
                <asp:Label ID="lb_venCode_cust" runat="server" Width="80px"></asp:Label>&nbsp;&nbsp;   
                <asp:Label ID="lb_venName" runat="server" Width="400px"></asp:Label>
             </td>
             
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                
                <asp:Label ID="Label13" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="App No" Width="100px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                <asp:Label ID="lb_appNo_cust" runat="server" Width="200px"></asp:Label>
             </td>
             
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label14" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Contract No" Width="100px" Height="16px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                <asp:Label ID="lb_cont_cust" runat="server" Width="100px"></asp:Label>              
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label17" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="ID No" Width="100px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:180px;height:24px;">
                <asp:Label ID="lb_id_no_cust" runat="server" Width="180px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                 <asp:Label ID="Label4" runat="server" Height="16px" 
                     style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="CIS No" 
                     Width="100px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:180px;height:24px;">
                <asp:Label ID="lb_csn_cust" runat="server" Width="100px"></asp:Label>
&nbsp;<asp:HiddenField ID="Hid_paymentType" runat="server" />
                 <asp:HiddenField ID="Hid_DebitBankCode" runat="server" />
                 <asp:HiddenField ID="Hid_BankBranchCode" runat="server" />
                 <asp:HiddenField ID="Hid_AccType" runat="server" />
                 <asp:HiddenField ID="Hid_Doc" runat="server" />
                 <asp:HiddenField ID="Hid_acc" runat="server" />
                 <asp:HiddenField ID="Hid_bank_sts" runat="server" />
                 <asp:HiddenField ID="Hid_approve_sts" runat="server" />
                 <asp:HiddenField ID="Hid_branch_cust" runat="server" />
             </td>
             
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label16" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Payment Type" Width="100px" Height="16px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="180px" 
                     ID="dd_payment_cust" AutoPostBack="true" 
                     OnSelectedIndexChanged="dd_payment_cust_SelectedIndexChanged">
                    <Items>
                        <dx:ListEditItem Value="1"  Text="Prepare Auto pay" />
                        <dx:ListEditItem Value="2"  Text="Bill Pay" />
                        <dx:ListEditItem Value="3"  Text="CS/Pay Point" />
                        <dx:ListEditItem Value="4"  Text="Ready Auto Pay" />
                        <dx:ListEditItem Value="5"  Text="Cheque" />
                        <dx:ListEditItem Value="W"  Text="Waiting for Approve" />
                        <dx:ListEditItem Value="R"  Text="Reject" />
                    </Items>
                </dx:ASPxComboBox>       
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label20" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Debit Bank code" Width="120px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:180px;height:24px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="180px" ID="dd_bank_code_cust" 
                     OnSelectedIndexChanged="dd_bank_code_cust_SelectedIndexChanged" AutoPostBack="true" DisabledStyle-BackColor="Silver">
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxComboBox>       
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
               <asp:Label ID="Label19" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Branch code" Width="100px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
               <dx:ASPxComboBox runat="server" ValueType="System.String" Width="200px" ID="dd_branch_code_cust" DisabledStyle-BackColor="Silver">
<DisabledStyle BackColor="Silver"></DisabledStyle>
                 </dx:ASPxComboBox>
             </td>
             
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label21" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Account Type" Width="100px" Height="16px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                <dx:ASPxComboBox runat="server" ValueType="System.String" Width="180px" ID="dd_accType_cust">
                    <DisabledStyle BackColor="Silver">
                    </DisabledStyle>
                 </dx:ASPxComboBox>         
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label22" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Doc No" Width="100px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:180px;height:24px;">
                <asp:TextBox ID="txt_doc_cust" runat="server" Width="180px" ></asp:TextBox>  
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
               <asp:Label ID="Label23" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Acc No" Width="100px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
               <asp:TextBox ID="txt_accNo_cust" runat="server" Width="200px" MaxLength="15" ></asp:TextBox> 
             </td>
             
        </tr>
        <tr>
            <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label24" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Bank Status" Width="100px" Height="16px"></asp:Label>
            </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:200px;height:24px;">
                <asp:TextBox ID="txt_bank_sts_cust" runat="server" Width="100px" ></asp:TextBox>          
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:100px;height:24px;">
                <asp:Label ID="Label25" runat="server" 
                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                    Text="Approve Status" Width="120px" Height="16px"></asp:Label>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:120px;height:24px;">
                 <asp:RadioButtonList ID="rb_approve_sts_cust" runat="server" RepeatDirection="Horizontal" Width="150px">
                    <asp:ListItem Value="A" Text="Approve"></asp:ListItem>
                    <asp:ListItem Value="R" Text="Reject"></asp:ListItem>
                 </asp:RadioButtonList>
             </td>
             <td style="text-align: left; font-family: Tahoma; font-size: small; width:220px;height:24px;" colspan="2">
                <dx:ASPxButton  ID="btn_save" runat="server" Height="24px"
                 SpriteCssFilePath="~/App_Themes/Office2003Blue/{0}/sprite.css"
                 CssPostfix="Office2003Blue"
                 CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                 Width="170px" OnClick="btn_save_Click">
                 <ClientSideEvents Click="function(s, e) {LoadingPanel.Show();}" />
                </dx:ASPxButton>                 
                 <%--<asp:Button runat="server" Text="" BackColor="#66CCFF" Height="24px" Width="180px" ID="btn_save" OnClick="btn_save_Click"></asp:Button>--%>
             </td>
             
        </tr>
    </table>                                                          
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
                                <dx:ASPxLabel ID="lblConfirmMsgEN" runat="server" Font-Names="Tahoma" 
                                    Font-Size="Small" Style="color: #800000"></dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:HiddenField ID="hid_oper" runat="server" />
                  
                    <br />
                    
                    <br /><br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="right" width="50%" >
                                <dx:ASPxButton ID="btnConfirmSave" runat="server" Text="OK" Width="100px" 
                                    OnClick="btnConfirmSave_Click" >
                                    <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
<ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </td>
                            <td align="left" width="50%" >
                                <dx:ASPxButton ID="btnConfirmCancel" runat="server" Text="Cancel" Width="100px" >
                                    <ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide(); }" />
<ClientSideEvents Click="function(s, e) { PopupConfirmSave.Hide();}"></ClientSideEvents>
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

            </td>
            </tr>
        </table>
</asp:Content>

