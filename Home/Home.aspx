<%@ Page Title="IL System" Language="C#" MasterPageFile="~/ManageData/MasterSite/CSManageMasterSiteMain.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CSManage_Home_Home" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register src="Home.ascx" tagname="Home" tagprefix="uc1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phContents" Runat="Server">
    
    <%-- BeginRegion ASPxRoundPanel --%>
     <%-- EndRegion --%>
           <script type="text/javascript">
               window.onresize = function () {
                   document.getElementById('myiframeid').height = getClientHeight() - 100;
                   document.getElementById('myiframeid').width = getClientWidth() - 20;

               }
      </script>
    <table heigth=100%; width=100%>
    <tr align=center><td align=center 
            style="width:100%; text-align: right; height: 100%;">
                  
                  <iframe id="myiframeid" name="myiframe"  src="../WorkProcess/SelectBranch/IL_branch.aspx" width="100%"
                        frameborder="0" align=center>
                           
				 </iframe>
                       <%-- <iframe id="myiframeid" name="myiframe"  src="../Images/Chg_Home.JPG" width="100%"
                        frameborder="0" align=center>
                           
						</iframe>--%>
<%--               
                        <iframe id="myiframeid" name="myiframe" uc1:Home ID="HomeWebControl" runat="server">
            			</iframe>--%>
               
    
          
          <%--      <uc1:Home ID="HomeWebControl" runat="server" />--%>
    
          
        </td>
        </tr>
        </table>
         
        <script language="JavaScript" type="text/JavaScript">

            document.getElementById('myiframeid').height = getClientHeight() - 100;
            document.getElementById('myiframeid').width = getClientWidth() - 20;
                       			 
		</script>
        
        
   <%-- <dx:ASPxPopupControl ID="PopupBranch" ClientInstanceName="PopupBranch"
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
                    <table width="100%" align="center" >
                        <tr>
                            <td align="center">
                                 <asp:Label ID="Label2" runat="server" 
                                                        style="font-family: Tahoma; font-size: small; font-weight: 700;" 
                                                        Text="Branch :" Width="100px">
                                                    </asp:Label>
                            </td>
                            <td align="center">
                                <dx:ASPxComboBox ID="dd_branch" runat="server" width="250px">
                                </dx:ASPxComboBox>
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
        </dx:ASPxPopupControl>--%>

    
</asp:Content>

