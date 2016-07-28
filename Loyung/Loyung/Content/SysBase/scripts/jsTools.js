//获取URL参数的值
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//JS去除空字符
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, '');
}
/** 格式化输入字符串**/
//用法: "hello{0}".format('world')；返回'hello world'
String.prototype.format = function () {
    var args = arguments; 
    if (this != window) {
        return this.replace(/\{(\d+)\}/g, function (s, i) {
            return args[i];
        });
    }
}
/*
 *时间戳转换为格式化时间
 */
function stamp2datetime(timestamp) {
    if (timestamp != null) {
        var date = new Date(parseInt(timestamp.replace("/Date(", "").replace(")/", ""), 10));
        //月份为0-11，所以+1，月份小于10时补个0
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
    return "";
}
//时间格式转化为时间戳
function datetime2stamp(datetime) {
    var tmp_datetime = datetime.replace(/:/g, '-');
    tmp_datetime = tmp_datetime.replace(/ /g, '-');
    var arr = tmp_datetime.split("-");
    var now = new Date(Date.UTC(arr[0], arr[1] - 1, arr[2], arr[3] - 8, arr[4], arr[5]));
    return parseInt(now.getTime() / 1000);
}
