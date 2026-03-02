<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TalentToolsbar.ascx.cs" Inherits="TalentToolsbar" %>
<style type="text/css">
    .style1
    {
        width: 15px;
        height: 4px;
    }
    .style2
    {
        width: 1px;
        height: 4px;
    }
    .style3
    {
        width: 7px;
        height: 4px;
    }
</style>
<table bgcolor="#F1F1EA" border="0" cellpadding="1" frame="border" style="border: 1px outset #C0C0C0;" width="210">
    <tr valign="top">
        <td style="text-align: center;" class="style3">
            <asp:ImageButton ID="btnSave" runat="server" Enabled="False" 
                ImageUrl="~/DataSecurity/Images/accept-32x32.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Save" />
            </td>
        <td style="text-align: center;" class="style2">
            <asp:Image ID="Image4" runat="server" Height="15px" 
                ImageUrl="~/DataSecurity/Images/bar.gif" Width="1px" Visible="False" />
        </td>
        <td style="text-align: center; " class="style1">
            <asp:ImageButton ID="btnCancel" runat="server" Enabled="False" 
                ImageUrl="~/DataSecurity/Images/ButtonRefresh.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Cancel" />
            </td>
        <td style="text-align: center;" class="style2">
            <asp:Image ID="Image5" runat="server" Height="20px" 
                ImageUrl="~/DataSecurity/Images/bar.gif" Width="1px" />
        </td>
        <td style="text-align: center; " class="style1">
            <asp:ImageButton ID="btnDelete" runat="server" Enabled="False" 
                ImageUrl="~/DataSecurity/Images/delete-32x32.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Delete" />
            </td>
        <td style="text-align: center; " class="style2">
            <asp:Image ID="Image6" runat="server" Height="20px" 
                ImageUrl="~/DataSecurity/Images/bar.gif" Width="1px" />
        </td>
        <td style="text-align: center; " class="style1">
            <asp:ImageButton ID="btnNew" runat="server" Enabled="False" 
                ImageUrl="~/DataSecurity/Images/add-32x32.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Add New" />
        </td>
        <td style="text-align: center; " class="style2">
            <asp:Image ID="Image3" runat="server" Height="20px" 
                ImageUrl="~/DataSecurity/Images/bar.gif" Width="1px" Visible="False" />
        </td>
        <td style="text-align: center; " class="style1">
            <asp:ImageButton ID="btnEdit" runat="server" Enabled="False" 
                ImageUrl="~/DataSecurity/Images/edit-32x32.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Edit" />
            </td>
        <td style="text-align: center; " class="style2">
            <asp:Image ID="Image2" runat="server" Height="20px" 
                ImageUrl="~/DataSecurity/Images/bar.gif" Width="1px" />
        </td>
        <td style="text-align: center; " class="style1">
            <asp:ImageButton ID="btnGotoList" runat="server" 
                ImageUrl="~/DataSecurity/Images/ButtonBack.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Go to List" Height="22px" />
            </td>
        <td style="text-align: center; " class="style2">
            <asp:Image ID="Image1" runat="server" Height="20px" 
                ImageUrl="~/DataSecurity/Images/bar.gif" Width="1px" Visible="False" />
        </td>
        <td style="text-align: center; " class="style1">
            <asp:ImageButton ID="btnSearch" runat="server" Enabled="False" 
                ImageUrl="~/DataSecurity/Images/ButtonSearch.png" OnClick="btnChooserToolsbar_Click" 
                ToolTip="Search" />
            </td>
    </tr>
</table>