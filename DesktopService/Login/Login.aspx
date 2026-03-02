<%@ Page Title="" Language="C#" MasterPageFile="~/DesktopService/MasterSite/SignOnMasterPage.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EBWebTemplate.DesktopService.Login.Login" %>
<%@ Import Namespace="System.Web.UI.WebControls"  %>
<%@ Import Namespace="System.Web.UI"  %>
<%@ Import Namespace="System.Web"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainDesktopContent" Runat="Server">
    <%--<title>eReserve Web System</title>--%>
    
        <script language="javascript" type="text/javascript">
            if (window.top != window) {
                window.top.location = window.location;
            }
	    function SetUpperText(vID){
	    
    		var obj = document.getElementById(vID); 
		    if(obj!=null)
			    obj.value = obj.value.toUpperCase();	  
	    }
        </script>

    <%--<span style="font-family: Tahoma; font-size: small; color: #FFFFFF">--%>
    <table width="100%">
    <tr>
        <td style="width: 165px">
            &nbsp;</td>
        <td colspan="2">
    <%--<span style="font-family: Tahoma; font-size: small; color: #FFFFFF">--%>
                
                <asp:Login ID="ctlLogin" runat="server" DisplayRememberMe="False" 
                    TitleText="User Authentication" OnAuthenticate="Login_Authenticate" 
                    DestinationPageUrl="~/DesktopService/Home/Home.aspx" BackColor="#EFF3FB" 
                    BorderColor="#B5C7DE" BorderStyle="Solid" BorderWidth="1px" 
                    Font-Names="Tahoma" Font-Size="Small" Height="113px" BorderPadding="4" 
                    ForeColor="#333333" Width="241px">
                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                    <TitleTextStyle BackColor="#507CD1" Font-Bold="True" 
                        ForeColor="#FFFFFF" Font-Size="0.9em" />
                    <TextBoxStyle Font-Size="0.8em" />
                    <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" 
                        BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                        ForeColor="#284E98" />
                </asp:Login>
            <%--</span>--%>
        </td>
        <td>

    <span style="font-family: Tahoma; font-size: small; color: #FFFFFF"> 
    <asp:ImageMap ImageUrl="~/ManageData/Images/MainSec.png" ID="imgMain" 
                SkinID="MainBanner" CssClass="MainBanner" ImageAlign="Bottom" runat="server" 
                style="text-align: right"></asp:ImageMap>               
            </span> </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</td>
        <td colspan="2">

    <span style="font-family: Tahoma; font-size: small; color: #FFFFFF">
            <asp:Label ID="Label1" runat="server" 
                Text="Version : 2022-11-29.000"></asp:Label>
            </span>                
        </td>
    </tr>
</table> 
    <%--</span>--%>           
</asp:Content>
