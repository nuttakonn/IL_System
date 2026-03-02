<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportCampaignProposal.aspx.cs" Inherits="ILSystem.ManageData.WorkProcess.Campaign.REPORT.reportCampaignProposal" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body> 
    <form id="form1" runat="server">
        <div style="background-color:#BAC9DD;" align="center">
            <asp:HiddenField ID="ds_gvCampaign" runat="server" />
            <table style="width:auto; align-content:center;">
                <tr>
                    <td>
                         <CR:CrystalReportViewer ID="crpCampaignProposal" runat="server" Width="100%" AutoDataBind="true" GroupTreeStyle-ShowLines="False" HasToggleGroupTreeButton="false" HasCrystalLogo="False" Height="50px" ToolPanelView="None" EnableDrillDown="False" HasDrilldownTabs="False"/>    
                    </td>
                </tr>
            </table>
          
        </div>
    </form>
      </body>
</html>
