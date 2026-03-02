<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarExPagePopup.aspx.cs" Inherits="EBPortal.WebControlEx.CalendarEx.CalendarExPagePopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
    function ChooseReturnToTextBox2(ctlReturn,Day,Month,Year)
{
    
	var aInput = window.parent.document.getElementsByTagName('input');
	var iLen = aInput.length;
	var iCount = 0;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(ctlReturn) >= 0 )
		{
			curElement.value=Day;
			break;
		}
			
	}
	//window.parent.document.getElementById['dwindow'].style.display="none";
	window.parent.dwindow.style.display="none"; 
}
    
    </script>
</head>
<body style="margin-left:0;margin-top:0">
    <form id="form1" runat="server" >
    
    <div>
        <asp:calendar id="cldShow" runat="server" Height="182px" Width="200px" 
            BorderStyle="None" BackColor="SteelBlue"
				PrevMonthText=" " CellPadding="1" CellSpacing="1" NextMonthText=" " 
            onselectionchanged="cldShow_SelectionChanged" 
            onprerender="cldShow_PreRender">
				<TodayDayStyle BorderWidth="1px" ForeColor="Blue" BorderStyle="Solid" BorderColor="LightPink" BackColor="LightPink"></TodayDayStyle>
				<SelectorStyle Font-Bold="True" BackColor="LightPink"></SelectorStyle>
				<DayStyle Font-Size="Smaller" ForeColor="Black" BackColor="White"></DayStyle>
				<NextPrevStyle Font-Size="XX-Small" ForeColor="Gold"></NextPrevStyle>
				<DayHeaderStyle Font-Size="Smaller" ForeColor="White" BackColor="LightSkyBlue"></DayHeaderStyle>
				<SelectedDayStyle Font-Bold="True" ForeColor="White" BorderColor="#0000C0" BackColor="LightPink"></SelectedDayStyle>
				<TitleStyle Font-Size="X-Small" ForeColor="White" BackColor="SteelBlue"></TitleStyle>
				<WeekendDayStyle ForeColor="Black" BorderStyle="None" BackColor="#DEEFFF"></WeekendDayStyle>
				<OtherMonthDayStyle Wrap="False" ForeColor="Silver"></OtherMonthDayStyle>
		</asp:calendar>
		<input id="control" type="hidden" runat="server" style="Z-INDEX: 102; LEFT: 5px; POSITION: absolute; TOP: 187px"/>
    </div>
    </form>
</body>
</html>

