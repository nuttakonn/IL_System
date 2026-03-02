<%@ Page Title="" Language="C#" MasterPageFile="~/ManageData/MasterSite/ChgManage_BlankSite.Master" AutoEventWireup="true" CodeBehind="IL_branch.aspx.cs" Inherits="ManageData_WorkProcess_SelectBranch_IL_branch" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="phContents" Runat="Server">
    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
        <PanelCollection>
        <dx:ASPxPopupControl ID="PopupBranch" ClientInstanceName="PopupBranch"
            runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides"
            HeaderText="Select Branch" CloseAction="CloseButton" AllowDragging="True" 
        AutoUpdatePosition="True" Modal="True" Width="450px" 
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
       SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">                        
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
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="center" width="100%" >
                                
                                
                            </td>                         
                        </tr>
                    </table>
                    <table width="100%" align="center" >
                        
                        <tr>
                            <td align="center">
                                 <asp:Label ID="Label2" runat="server" 
                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                        Text="Branch :" Width="100px">
                                                    </asp:Label>
                            </td>
                            <td align="center">
                                <div style="float:left; ">
                                <dx:ASPxComboBox ID="dd_branch" runat="server" width="250px">
              
                                </dx:ASPxComboBox>
                                </div>
                                <div style="float:left; ">&nbsp;&nbsp;</div>
                                <div style="float:right; ">
                                    <dx:ASPxButton ID="btn_refresh" runat="server" RenderMode="Link" 
                                    Width="30px" Height="15px" AutoPostBack="true" ImagePosition="Right" 
                                    Image-Url="~/Images/refresh.png" Image-Height="15" Image-Width="15" 
                                    OnClick="btnLinkImageAndText_Click">
                                    <Image Height="15px" Url="~/Images/refresh.png" Width="15px">
                                    </Image>
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                        
                    </table>
                    <br />
                    <table width="100%" align="center">
                        <tr>                            
                            <td align="center" width="100%" >
                               
                                <dx:ASPxButton runat="server" Text="OK" ID="btn_ok" OnClick="btn_ok_Click" Width="100px">
                                      <ClientSideEvents Click="function(s, e) { PopupBranch.Hide(); }" />
                                </dx:ASPxButton>
                            
                                
                            </td>                         
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        </PanelCollection>
    </dx:PanelContent>
</asp:Content>
