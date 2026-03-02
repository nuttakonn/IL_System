<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Home.ascx.cs" Inherits="ManageData_Home_Home" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>


<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>

<table class="style1">
    <tr align=center>
        <td>
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                GroupBoxCaptionOffsetY="-28px" 
                SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="99%" 
                HeaderText="CS Data Setup Management System" HorizontalAlign="Center">
                <ContentPaddings Padding="14px" />
                <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Overline="False" 
                    Font-Size="Large" Font-Underline="False" />
                <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <asp:Image ID="Image1" runat="server" 
        ImageUrl="~\ManageData\Images\Home_img.jpg" Height="365px" Width="517px" />
                    </dx:PanelContent>
</PanelCollection>
                <BackgroundImage HorizontalPosition="center" VerticalPosition="top" />
            </dx:ASPxRoundPanel>
        </td>
    </tr>
</table>
