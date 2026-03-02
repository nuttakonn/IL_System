<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DesktopService/MasterSite/EBWebSite.Master" CodeBehind="Home.aspx.cs"  %>
<%--<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/DesktopService/MasterSite/EBWebSite.Master" CodeBehind="Home.aspx.cs" Inherits="EBWebTemplate.DesktopService.MainMenu" %>--%>
<%@ Register Src="~/DesktopService/MainMenu/MainMenu.ascx" TagName="uclPortalMenu" TagPrefix="ucp"%>
<asp:Content ID="HomeContent" ContentPlaceHolderID="MainDesktopContent" runat="server" >
    <div class="RootPage">
        <div class="Slogan" style="height: 17px">&nbsp;<span 
                style="font-family: Tahoma; font-size: small">EasyBuy Web Application Portal [connect iSeries]</span><br /></div><br/><br/>
        <table width="99%">
        <tr>
        <td valign="top" style="text-align: left"><ucp:uclPortalMenu ID="ucpPortal" runat="server" />        </td>
        <td valign="top" style="text-align: right" ><asp:ImageMap ImageUrl="~/Images/Main.png" ID="imgMain" SkinID="MainBanner" CssClass="MainBanner" ImageAlign="Right" runat="server"></asp:ImageMap></td>        
        </tr>
        
        
        </table>


        <asp:Panel ID="pDescription" EnableViewState="False" runat="server" />
    </div>
</asp:Content>