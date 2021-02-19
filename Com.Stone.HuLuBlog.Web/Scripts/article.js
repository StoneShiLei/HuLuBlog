

layui.use(['jquery', 'layer'], function () {
    var $ = layui.jquery;
    var layer = layui.layer;

    $('.search-btn').click(function () {
        GetDataByHtml($(this).data("request-url"), "get", { keyword: $(".keywords").val() }, function (data) {
            $(".blog-main-left").html(data);
            $('html,body').animate({ scrollTop: 0 }); //滚动条置顶
            $(".keywords").val = "";
        });
    });

    $(document).keyup(function (event) {
        if (event.keyCode == 13) {
            $('.search-btn').click();
        }
    });
});