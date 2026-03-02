<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Note.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Note.Note" %>
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
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server" >
     <ajaxtoolkit:toolkitscriptmanager runat="server" EnablePartialRendering="true" 
    EnableScriptLocalization="true" EnableScriptGlobalization="true"  
        ID="ScriptManager1" AsyncPostBackTimeout="36000" />
    <asp:UpdatePanel runat="server" ID="UpdatePanel1"  >
                    
                    <ContentTemplate>
    <div>
       
       <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                    GroupBoxCaptionOffsetY="-28px" 
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100%" 
                    Height="136px">
                    <ContentPaddings Padding="5px" />
<ContentPaddings Padding="5px"></ContentPaddings>
                    <HeaderTemplate>
                        <span style="font-size: small">ADD NOTE</span>
                    </HeaderTemplate>
                    <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
  
                    <table width="100%" align="center">
                        <tr>
                            <td align="right" class="style2" style="text-align: right; width: 100px;">
                                <asp:Label ID="Label1" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Name:" Width="100px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <asp:Label ID="lb_Fname" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Width="150px" ForeColor="Blue"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label2" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" Text="ID: " 
                                Width="15px"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lb_id" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                Width="150px" ForeColor="Blue"></asp:Label>
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
                                
                                <asp:Label ID="Label4" runat="server" 
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
                                <div style="float:left">&nbsp;&nbsp;</div>
                                 
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style2" style="text-align: right; width: 100px;">
                                <asp:Label ID="Label6" runat="server" 
                                    style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                    Text="Result code:" Width="100px"></asp:Label>
                            </td>
                            <td class="style6" style="text-align: left;">
                                <div style="float:left">
                                    <dx:ASPxComboBox ID="dd_ResCode" runat="server"  dropdownrows="10" 
                                    dropdownstyle="DropDown" incrementalfilteringmode="StartsWith"   
                                    backcolor="#FFFFC4" valuetype="System.String" width="300px">
                                      
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
                                <asp:HiddenField ID="hid_csn" runat="server" />
                                <asp:HiddenField ID="hid_idn" runat="server" />
                           </td>                   
                            <td align="left">
                                <div style="float:left">
                                    <dx:ASPxButton ID="btn_saveNote" runat="server" Text="OK" Width="100px"  
                                         OnClick="btn_saveNote_Click" >
                                      <ClientSideEvents Click="function(s, e) {LoadingPanel.Show();}" />
                                    </dx:ASPxButton>
                                </div>
                                <div style="float:left">&nbsp;&nbsp;</div>
                                <div style="float:left">
                                    <dx:ASPxButton ID="btn_cancel_Save" runat="server" Text="Cancel" Width="100px" 
                                        CausesValidation="false" >

                                        <ClientSideEvents Click="function(s, e) {close ();}" />

                                    </dx:ASPxButton>
                                </div>
                                <div style="float:left">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="red" Font-Bold="true" Width="250px"></asp:Label>
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
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" ClientInstanceName="LoadingPanel" runat="server" Width="250px" Height="150px" 
    CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" >
    </dx:ASPxLoadingPanel> 
      </dx:PanelContent>
</PanelCollection>
            <Border BorderColor="#8B8B8B" BorderStyle="Solid" BorderWidth="1px" />
        </dx:ASPxRoundPanel>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
</form>
</body>
</html>

