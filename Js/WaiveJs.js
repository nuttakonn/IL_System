var hours = 0; //document.getElementById('HidHours').value;
var minutes = 0;  //document.getElementById('HidMinutes').value;
var seconds = 3; // document.getElementById('HidSeconds').value;
var countdown = 0;
function display() {
    if ((hours <= 0) && (minutes <= 0) && (seconds <= 0)) {
        window.open('', '_self');
        self.close();

        return;
    }
    if (seconds <= 0) {
        if ((hours == 0) && (minutes == 0))
            seconds = 0;
        else {
            seconds = 60;
            minutes -= 1;
        }
    }
    if (minutes <= 0) {
        if ((hours < 0) && (minutes < 0)) {
            hours = minutes = seconds = 0;
        }
        else {
            if ((hours == 0) && (minutes == 0))
                hours = minutes = 0;
            if ((hours > 0) && (minutes < 0)) {
                minutes = 59;
                hours -= 1;
            }
        }
    }
    if ((minutes <= -1) || (hours <= -1)) {
        if (hours <= -1) {
            minutes = 0;
            hours += 1;
        }
        else
            minutes -= 1;
        seconds = 0;
        minutes += 1;
    }
    else
        if (seconds > 0)
        seconds -= 1;
    //document.getElementById('counter').value = hours + ":" + minutes + ":" + seconds;
    document.getElementById('counter').value = "Window will be closed in " + seconds + " seconds please wait...";
    setTimeout("display()", 1000);

}