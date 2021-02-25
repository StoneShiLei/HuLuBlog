layui.use(['jquery','form','layer'], function () {
    var $ = layui.jquery;
    var form = layui.form;
    var layer = layui.layer;

    form.render();

    //自定义验证规则
    form.verify({
    pass: [
        /^[\S]{8,16}$/
        ,'密码必须8到16位，且不能出现空格'
    ]
    ,content: function(value){
        layedit.sync(editIndex);
    }
    });

    form.on('submit(login)',function(data){
        if(data.field.isRemember == "on") 
            data.field.isRemember = true;
        else
            data.field.isRemember = false;

        if (!data.field.recaptchaToken) {
            layer.msg("请等待人机验证")
        }
        else {
            GetDataByJson($(this).data("request-url"), 'post', data.field, function (data) {
                if (data.IsSuccess) {
                    window.parent.location.href = $(".button-login").data("home-url") //跳转主页
                    var index = parent.layer.getFrameIndex(window.name)//获取窗口索引
                    parent.layer.close(index) //关闭弹出窗
                } else {
                    layer.msg(data.Message);
                }

            });
        }
    });


});