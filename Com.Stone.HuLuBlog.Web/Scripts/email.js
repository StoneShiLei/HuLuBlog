layui.use(['jquery', 'form', 'layer'], function () {
    var $ = layui.jquery;
    var form = layui.form;
    var layer = layui.layer;

    form.render();

    form.on('submit(send)', function (data) {
        if (!data.field.recaptchaToken) {
            layer.msg("请等待人机验证")
        }
        else {
            GetDataByJson($(this).data("request-url"), 'post', data.field, function (data) {
                if (data.IsSuccess) {
                    var index = parent.layer.getFrameIndex(window.name)//获取窗口索引
                    parent.layer.close(index) //关闭弹出窗
                    parent.layer.msg(data.Message);
                } else {
                    layer.msg(data.Message);
                }

            });
        }
    });


});