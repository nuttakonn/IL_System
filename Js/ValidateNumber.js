// --fix number -- 
function chkNumber(ele) {
    var vchar = String.fromCharCode(event.keyCode);
    if ((vchar < '0' || vchar > '9') && (vchar != '.')) {
        return false;
    }
     
    ele.onKeyPress = vchar;
 
}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}
// 2 position
function chkNum(ele) {
    var num = parseFloat(ele.value);

    if (isNaN(num)) {

        return 0;
        
    } else {
        ele.value = addCommas(num.toFixed(2));  
    } 
}
// 3 position
function chkNum3(ele) {
    var num = parseFloat(ele.value);

    if (isNaN(num)) {

        return 0;

    } else {
        ele.value = addCommas(num.toFixed(3));
    }
}

function chkNumberMinus(ele) {
    var vchar = String.fromCharCode(event.keyCode);
    if ((vchar < '0' || vchar > '9') && (vchar != '.') && (vchar != '-')) {
        return false;
    }
    ele.onKeyPress = vchar;
}

function chkNumberOnly(ele) {
    var vchar = String.fromCharCode(event.keyCode);
    if ((vchar < '0' || vchar > '9')) {
        return false;
    }
    ele.onKeyPress = vchar;
}

function chkNumberMultiTerm(ele) {
    var vchar = String.fromCharCode(event.keyCode);
    if ((vchar < '0' || vchar > '9') && (vchar != ',') && (vchar != '-')) {
        return false;
    }
    ele.onKeyPress = vchar;
}