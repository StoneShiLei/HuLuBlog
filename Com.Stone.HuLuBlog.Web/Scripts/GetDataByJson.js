function GetDataByJson(url, method, data, callback) {
    $.ajax({
        type: method,
        url: url,
        data: data,
        async: true,
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded',
        //headers: {
        //    "Token": window.localStorage.getItem("token")
        //},
        beforeSend: function () {
            layer.load(); //上传loading
        },
        complete: function () {
            layer.closeAll('loading');
        },
        success: callback,
        error: function (jqXHR) {

            layer.msg("请求服务器发生异常，请联系系统管理员或者稍后再试!");
            return "";
        }
    });
}

function GetDataByHtml(url, method, data, callback) {
    $.ajax({
        type: method,
        url: url,
        data: data,
        async: true,
        dataType: "html",
        contentType: 'application/x-www-form-urlencoded',
        //headers: {
        //    "Token": window.localStorage.getItem("token")
        //},
        beforeSend: function () {
            layer.load(); //上传loading
        },
        complete: function () {
            layer.closeAll('loading');
        },
        success: callback,
        error: function (jqXHR) {

            layer.msg("请求服务器发生异常，请联系系统管理员或者稍后再试!");
            return "";
        }
    });
}