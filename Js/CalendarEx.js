
var objPopWindows;
var initialwidth,initialheight;
function CallPagePopup(strPathCal)
{
		
	objPopWindows = window.open( strPathCal,'cal','width=498,height=485,left=100,top=200');
	if(window.focus)
		objPopWindows.focus;
}

function PickUpCalendar(strPathCal,strFormatDate,ctSource,anchorName)
{
    var win = new PopupWindow();
    win.setSize(198,185);
    win.setUrl(strPathCal+"/CalendarPagePop.aspx?FormatDate="+strFormatDate+"&ctlParent="+ctSource);
    win.showPopup(anchorName);
}
function CallCalendarPagePopup(strPathCal,strFormatDate,ctSource,obj)
{
	var newX = findPosX(obj)+20;
	var newY = findPosY(obj); 
	if (objPopWindows!=null)
		objPopWindows.focus;
		
	objPopWindows = window.open( strPathCal + "/CalendarPagePop.aspx?FormatDate=" 
					+ strFormatDate + "&ctlParent=" + ctSource 
					,'cal',"width=205,height=185,left=" + newX.toString() + ",top=" + newY.toString());
	if(window.focus)
		objPopWindows.focus;


}
function Div_Onblur()
{
	var div = FindControl('DIV','dwindow');
	
	if(div !=null)
	{
		//div.style.display='';
	   //return;
	   document.forms['aspnetForm'].removeChild(div);
	}	

}
function CallCalendarPagePopup2(strPathCal,strFormatDate,ctSource) //,obj)
{
	alert('');
	var newX = findPosX(obj)+20;
	var newY = findPosY(obj)-100;
	var div = FindControl('DIV','dwindow');
	
	if(div !=null)
	{
		//div.style.display='';
	   //return;
	   document.forms['aspnetForm'].removeChild(div);
	}
	
	div = document.createElement('DIV');
	div.id = 'dwindow';
	div.style.position='absolute';
	div.style.display='';
	div.style.width=initialwidth=200;
	div.style.height=initialheight=182;
	div.style.left=newX;
	div.style.top=newY;  
			
			
	var frm = document.createElement('iframe');
	frm.id = 'cframe';
	frm.width='100%';
	frm.height='100%';
	frm.frameBorder=0;
	frm.src = strPathCal + "/CalendarExPagePopup.aspx?FormatDate=" 
			+ strFormatDate + "&ctlParent=" + ctSource;

	document.forms['aspnetForm'].appendChild(div).appendChild(frm);
			
	/*
	window.document.getElementById("dwindow").style.display=''
    window.document.getElementById("dwindow").style.width=initialwidth=210
    window.document.getElementById("dwindow").style.height=initialheight=190
    window.document.getElementById("dwindow").style.left=newX
    window.document.getElementById("dwindow").style.top=newY
    window.document.getElementById("cframe").src=strPathCal + "/CalendarPagePop.aspx?FormatDate=" 
					+ strFormatDate + "&ctlParent=" + ctSource;
	*/
	/*
	var newX = findPosX(obj)+20;
	var newY = findPosY(obj); 
	if (objPopWindows!=null)
		objPopWindows.focus;
		
	objPopWindows = window.open( strPathCal + "/CalendarPagePop.aspx?FormatDate=" 
					+ strFormatDate + "&ctlParent=" + ctSource 
					,'cal',"width=205,height=185,left=" + newX.toString() + ",top=" + newY.toString());
	if(window.focus)
		objPopWindows.focus;
		
		//objPopWindows.focus;
	*/	
	/*
	var newX = findPosX(obj)+20;
	var newY = findPosY(obj); 
	if (objPopWindows!=null)
		objPopWindows.close;
		
	objPopWindows = window.open( strPathCal + "/CalendarPagePop.aspx?FormatDate=" 
					+ strFormatDate + "&ctlParent=" + ctSource + "&ctlType=" + ctType 
					,'cal',"width=198,height=185,left=" + newX.toString() + ",top=" + newY.toString());
	if(window.focus)
		objPopWindows.focus;
	*/
}

function FindControl(strTagType,strName)
{
	var aInput = document.forms['aspnetForm'].tags(strTagType);
	var iLen = aInput.length;
	var Ret;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(strName) > -1 )
		{
			Ret = curElement;
			break;
		}
	}
	return Ret;
}

function findPosX(obj)
{
	var offset=0;
	while (obj != null && obj != document.body)
	{
		offset += obj.offsetLeft;
		obj = obj.offsetParent;
	}
	return offset + window.screenLeft;
}

function findPosY(obj)
{
	var offset=0;
	while (obj != null && obj != document.body)
	{
		offset += obj.offsetTop;
		obj = obj.offsetParent;
	}
	return offset + window.screenTop;
}
function ChooseReturnToTextBox2(ctlReturn,Day,Month,Year)
{
    
	var aInput = parent.document.forms['aspnetForm'].tags('input');
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
	parent.dwindow.style.display="none"; 
}
function ChooseReturnToTextBox(ctlReturn,Day,Month,Year)
{
	var aInput = window.opener.document.all.tags('input');
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
	self.close();
	
}
