<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="EBWebTemplate.DesktopService.MainMenu.MainMenu" %>
 <asp:DataList ID="dviewMainMenu" runat="server" Width="100%" 
    CssClass="Features" ColumnCount="1" ItemSpacing="43px" SkinID="None" 
    RowPerPage="100" onitemdatabound="dviewMainMenu_ItemDataBound" >
            
            <ItemTemplate>
                
            </ItemTemplate>
            <ItemStyle BorderStyle="None" BorderWidth="10px" Width="50%" VerticalAlign="Top" />
        
        </asp:DataList>