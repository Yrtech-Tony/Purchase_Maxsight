//字符产格式
function ChangeDateFormat(d) {
    if (!d) return "";
    if (d == '1970-01-01T00:00:00') return "";
    var date = new Date(parseInt(d.replace("/Date(", "").replace(")/", ""), 10));
    console.log(date)
    var month = padLeft(date.getMonth() + 1, 10);
    var currentDate = padLeft(date.getDate(), 10);
    var hour = padLeft(date.getHours(), 10);
    var minute = padLeft(date.getMinutes(), 10);
    return date.getFullYear() + "-" + month + "-" + currentDate;// + " " + hour + ":" + minute;
}
function padLeft(str, min) {
    if (str >= min)
        return str;
    else
        return "0" + str;
}