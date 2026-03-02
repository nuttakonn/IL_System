function ConfirmDelete()
{
	return confirm("Are you sure want to delete this item?");
}
function ConfirmSave()
{
	if (confirm("Are you sure want to save this item?"))
	    {
	    if (typeof(Page_ClientValidate) == 'function') 
	        {
	        Page_ClientValidate();
	        return true;
	        }
	    }
	else
	    return false;
}
//Peak 02/11/2550 เพิ่ม Script สำหรับการส่งค่า Wording ให้กับ Script Confirm
function ConfirmSaveWording(wording)
{
	if (confirm(wording))
	    {
	    if (typeof(Page_ClientValidate) == 'function') 
	        {
	        Page_ClientValidate();
	        return true;
	        }
	    }
	else
	    return false;
}

function ConfirmCancel()
{
	return confirm("Are you sure want to cancel this item?");
}
function ConfirmGenerate()
{
	return confirm("Are you sure want to Generate Data ?");
}
function ConfirmReGenerate()
{
	return confirm("Are you sure want to Re-Generate Data ?");
}
function ConfirmImportLoc()
{
	return confirm("Are you sure want to Import Box Location?");
}
function ConfirmImportPart()
{
	return confirm("Are you sure want to Import Box Part?");
}
function ConfirmStockintxt()
{
	return confirm("Are you sure want to Import StockIn?");
}

function OpenNewWindow(url)
{
	window.open(url);
	return false;
}

function OpenWindow(url, winname, width, height, fld )
{
	window.open(url, winname, 'titlebar=no,scrollbars=yes,width='+width+',height='+height+',left='+Math.min(screen.availWidth-width,getObjectLeft(fld))+',top='+Math.min(screen.availHeight-height,getObjectTop(fld)));
	return false;
}

function OpenWindowNewChildObject(url, winname, width, height )
{
	window.open(url, winname, 'titlebar=no,scrollbars=yes,width='+900+',height='+500+',left='+(screen.availWidth-910)+',top='+(screen.availHeight-550));
	return false;
	
}

function OpenWindowPickup(url, winname )
{
	window.open(url, winname, 'titlebar=no,scrollbars=yes,width='+950+',height='+500+',left='+(screen.availWidth-960)+',top='+(screen.availHeight-550));
	return false;
}
function OpenWindowPickup_attach(url, winname )
{
	window.open(url, winname, 'titlebar=no,scrollbars=no,width='+500+',height='+100+',left='+(screen.availWidth-960)+',top='+(screen.availHeight-550));
	return false;
}

function OpenWindowAttachFile(url, winname )
{
	window.open(url, winname);
	return false;
}

function OpenWindowPickupValue( setUrlStr, anchorName )
{
    var win = new PopupWindow();
    win.setSize(400,400);
    win.setUrl(setUrlStr);
    win.showPopup(anchorName);
    return false;
}

function PickUpToDropdownList(target,value)
{
	var aInput = window.opener.document.all.tags('select');
	var iLen = aInput.length;
	var iCount = 0;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(target) >= 0 )
		{
			curElement.value=value;
			break;
		}
			
	}
	self.close(); 
}

function PickUpToTextBox(target, value)
{
	//alert('taget:=' + target) ;
	//alert('value:=' + value);
	var aInput = window.opener.document.all.tags('input');
	var iLen = aInput.length;
	var iCount = 0;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(target) >= 0 )
		{
			curElement.value=value;
			break;
		}
			
	}
	self.close(); 
}

function PickUpToTextBoxResell(tgAuctionLot,tgAuctionDateFrom, tgAuctionDateTo, valueAuctionLot, valueAuctionDateFrom, valueAuctionDateTo)
{
	//alert('taget:=' + target) ;
	//alert('value:=' + value);
	var aInput = window.opener.document.all.tags('input');
	var iLen = aInput.length;
	var iCount = 0;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(tgAuctionLot) >= 0 )
		{
			curElement.value=valueAuctionLot;
			//break;
		}
		else if(curElement.id.indexOf(tgAuctionDateFrom) >= 0 )
		{
			curElement.value=valueAuctionDateFrom;
			//break;
		}
		else if(curElement.id.indexOf(tgAuctionDateTo) >= 0 )
		{
			curElement.value=valueAuctionDateTo;
			//break;
		}
			
	}
	self.close(); 
}

//function PickUpToLocation(tgTumbol,tgAmphur,tgProvince,valueofTumbol,valueofAmphur,valueofProvince)
function PickUpToInsurance(tgInvoice,tgInsurance,valueofInvoice,valueofInsurance)
{
	//alert('tgInvoice:= ' + tgInvoice) ;
	//alert('valueofInvoice:= ' + valueofInvoice);
	//alert('tgInsurance:= ' + tgInsurance) ;
	//alert('valueofInsurance:= ' + valueofInsurance);
	var aInput = window.opener.document.all.tags('input');
	var iLen = aInput.length;
	var iCount = 0;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		
		if(curElement.id.indexOf(tgInvoice) >= 0)
		{
			
			curElement.value=valueofInvoice;
			
		}
		else if (curElement.id.indexOf(tgInsurance) >= 0)
		{
			curElement.value=valueofInsurance;
			
		
		}
			
	}
	self.close(); 
}
//function PickUpToDDLAddress(tgTumbol,tgAmphur,tgProvince,valueofTumbol,valueofAmphur,valueofProvince)  //,valueofZip
function PickUpToDDLAddress(tgTumbolCode,tgTambolDes,tgAmphurCode,tgAmphurDes,tgProvinceCode,tgProvinceDes,tgZip,valueofTumbolCode,valueofTambolDes,valueofAmphurCode,valueofAmphurDes,valueofProvinceCode,valueofProvinceDes,valueofZip)  
{
	//var aInput = window.opener.document.all.tags('select');
	var aInput = window.opener.document.all.tags('input');
	var iLen = aInput.length;
	var iCount = 0;
	
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		
		if(curElement.id.indexOf(tgTumbolCode) >= 0)
		{
			curElement.value=valueofTumbolCode;
		}
		else if(curElement.id.indexOf(tgTambolDes) >= 0)
		{
			curElement.value=valueofTambolDes;
		}
		else if (curElement.id.indexOf(tgAmphurCode) >= 0)
		{
			curElement.value=valueofAmphurCode;
		}
		else if (curElement.id.indexOf(tgAmphurDes) >= 0)
		{
			curElement.value=valueofAmphurDes;
		}
		else if( curElement.id.indexOf(tgProvinceCode) >= 0)
		{
			curElement.value=valueofProvinceCode;
		}
		else if( curElement.id.indexOf(tgProvinceDes) >= 0)
		{
			curElement.value=valueofProvinceDes;
		}
		else if( curElement.id.indexOf(tgZip) >= 0)
		{
			curElement.value=valueofZip;
		}
		
			
	}
	self.close(); 
}
function PickUpTest(tgBrandCode,valBrandCode)
{
	var aInput = window.opener.document.all.tags('input');
	var iLen = aInput.length;
	var iCount = 0;
	alert(iLen);
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(tgBrandCode) >= 0)
		{
			alert('BrandCode value: ' + valBrandCode);	
			curElement.value=valBrandCode;
			break;
		}
	}
	self.close();
}
function PickUpToBodyPart(tgBrandCode,tgBrandDesc,tgModelCode,tgModelDes,valBrandCode,valBrandDesc,valModelCode,valModelDes)
{
	var aInput = window.opener.document.all.tags('input');
	var iLen = aInput.length;
	var iCount = 0;
	
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		
		if(curElement.id.indexOf(tgBrandCode) >= 0)
		{
			alert('BrandCode value: ' + valBrandCode);	
			curElement.value=valBrandCode;
			
		}
		else if (curElement.id.indexOf(tgBrandDesc) >= 0)
		{
			alert('BrandDesc value: ' + valBrandDesc);
			curElement.value=valBrandDesc;
			
		}
		
		else if( curElement.id.indexOf(tgModelCode) >= 0)
		{
			alert('ModelCode value: ' + valModelCode);
			curElement.value=valModelCode;
			
		}
		else if( curElement.id.indexOf(tgModelDesc) >= 0)
		{
			
			curElement.value=valModelDesc;
			
		}
		
	}
	
	self.close(); 
}


function PickUpTo(tag, target, value)
{
	var aInput = window.opener.document.all.tags(tag);
	var iLen = aInput.length;
	var iCount = 0;
	for(i=0; i<iLen; i++)
	{
		var curElement = aInput[i];
		if(curElement.id.indexOf(target) >= 0 )
		{
			curElement.value=value;
			break;
		}
			
	}
	self.close(); 
}

function getObjectLeft(obj)
{
	var offset=0;
	while (obj != null && obj != document.body)
	{
		offset += obj.offsetLeft;
		obj = obj.offsetParent;
	}
	return offset + window.screenLeft;
}


function getObjectTop(obj)
{
	var offset=0;
	while (obj != null && obj != document.body)
	{
		offset += obj.offsetTop;
		obj = obj.offsetParent;
	}
	return offset + window.screenTop;
}


function  checkDDL_value(source,args )
{
if (args.Value == '0') 
{
args.IsValid=false;

}
else 
{
args.IsValid=true ;

}
}

function checkDDL_index(source,args)
{
var dropdown= document.getElementById(source.controltovalidate);
if (dropdown.selectedIndex == 0) 
{
args.IsValid=false;
}
else 
{
args.IsValid=true ;

}


}

function getObjectTopWindow(obj) {
    var offset = 0;
    while (obj != null && obj != document.body) {
        offset += obj.offsetHeight;
        obj = obj.offsetParent;
    }
    return offset;
}


function getClientWidth() {
    return document.compatMode == 'CSS1Compat' && !window.opera ? document.documentElement.clientWidth : document.body.clientWidth;
}

function getClientHeight() {
    return document.compatMode == 'CSS1Compat' && !window.opera ? document.documentElement.clientHeight : document.body.clientHeight;
}
function getAppPath() {
    var pathArray = location.pathname.split('/');
    var appPath = "/";
    for (var i = 1; i < pathArray.length - 1; i++) {
        appPath += pathArray[i] + "/";
        break;
    }
    return appPath;
}

function ViewInquiryAllType(request_no, request_type) {
    var url = getAppPath() + "Waive/Inquiry/" + GetRequestType(request_type) + "Inquiry/" + GetRequestType(request_type) + "InquiryForm_View.aspx?request_no=" + request_no;
    //alert(url);
    window.open(url, '', 'width=800,height=500,left=50,top=20,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes');
}

function GetRequestType(request_type) {
    if (request_type == "01") {
        return "Penalty";
    }
    if (request_type == "02") {
        return "Expense";
    }
    if (request_type == "03") {
        return "Loss";
    }
    if (request_type == "04") {
        return "SpecialDsWo";
    }
    if (request_type == "05") {
        return "Refund";
    }
    // Add type for negotiate
    if (request_type == "06" || request_type == "07" || request_type == "08" || request_type == "09" || request_type == "10") {
        return "Negotiate";
    }
    if (request_type == "90") {
        return "Cancel";
    }
}

