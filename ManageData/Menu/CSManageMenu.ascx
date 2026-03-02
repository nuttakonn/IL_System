<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CSManageMenu.ascx.cs" Inherits="CSManage.ManageData.Menu.CSManageMenu" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div>
    
<dxm:ASPxMenu EnableViewState="False" EnableClientSideAPI="True" OnItemClick="ASPxMenuSignOut_ItemClick"
        ID="ASPxSystemMenu" AutoSeparators="RootOnly" runat="server" 
        ShowPopOutImages="True" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
        CssPostfix="Aqua" GutterImageSpacing="7px" 
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
<%-- BeginRegion Items --%>
    <Items >
        <dxm:MenuItem Text="Home" Name="home" NavigateUrl="../Home/Home.aspx">
            
        </dxm:MenuItem>
        <dxm:MenuItem Text="Item Template" Name="item">			                                
            <SubMenuStyle Paddings-Padding="8px" Font-Size="10px" Font-Names="Verdana" 
                Wrap="False" ForeColor="#000000" >
<Paddings Padding="8px"></Paddings>
            </SubMenuStyle>
            <Items>
                <dxm:MenuItem Text="ASP.NET" Name="aspdotnet">
<%-- EndRegion --%>
<%-- BeginRegion MenuItemTemplate --%>
                    <Template>
                        						                    
                    </Template>
<%-- EndRegion --%>
<%-- BeginRegion Items --%>
                </dxm:MenuItem>
                <dxm:MenuItem Text="Announcements" Name="announcements">
                    <Template>
                        				                    
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Text="Building Controls" Name="building">
                    <Template>
                        		                    
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Text="Graphic Designer" Name="graphic">
                    <Template>
                        			                    
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Text="Web Controls" Name="webcontrols">
                    <Template>
                        			                    
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Text="Web Services" Name="webservices">
                    <Template>
                        				                    
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Text="Email" Name="email" BeginGroup="true">
                    <Template>
                                
                    </Template>
                </dxm:MenuItem>
            </Items>
        </dxm:MenuItem>
    </Items>
<%-- EndRegion --%>
    <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
    </LoadingPanelImage>
    <RootItemSubMenuOffset FirstItemX="-1" FirstItemY="-1" X="-1" Y="-1" />
    <ItemStyle DropDownButtonSpacing="12px" PopOutImageSpacing="18px" 
        ToolbarDropDownButtonSpacing="5px" ToolbarPopOutImageSpacing="5px" 
        VerticalAlign="Middle" />
    <SubMenuStyle GutterWidth="0px" />
    <ClientSideEvents ItemClick="function(s, e) {
    e.processOnServer = e.item.GetItemCount() == 0;
           }" />
</dxm:ASPxMenu>
</div>