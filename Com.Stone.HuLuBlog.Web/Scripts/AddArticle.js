$(function () {
    layui.use(['jquery', 'form', 'layer'], function () {
        var $ = layui.jquery;
        var form = layui.form;
        var layer = layui.layer;

        var editor = editormd("editor", { //id,后续name要开头一致
            width: "100%",
            height: 800,
            syncScrolling: "single",
            path: "../editor.md/lib/",  //插件路径
            saveHTMLToTextarea: true,  //影响第二个text能否获取html内容
            emoji: true,  //开启emoji
            flowChart: true,//开启流程图支持
            sequenceDiagram: true,//开启时序/序列图支持

            //图片上传
            imageUpload: true,
            imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
            imageUploadURL: $("#editor").data("request-url")
        });

        //剪切板截图上传
        $("#editor").on("paste", function (event) {
            var items = (event.clipboardData || event.originalEvent.clipboardData).items
            var file = null;
            if (items && items.length) {

                // 检索剪切板items
                for (var i = 0; i < items.length; i++) {
                    if (items[i].type.indexOf('image') !== -1) {
                        file = items[i].getAsFile();
                        break;
                    }
                }
            }
            if (file) {
                var formData = new FormData();
                formData.append('file', file);
                $.ajax({
                    url: $("#editor").data("request-url"),
                    type: 'post',
                    data: formData,
                    async: false,
                    contentType: false,//上传对象类型
                    processData: false, //上传对象类型
                    dataType: 'json',
                    success: function (res) {
                        if (res.success == 1)
                            editor.insertValue("\n![](" + res.url + ")");
                        else
                            layer.msg("图片上传失败")
                    },
                    error: function () {
                        layer.msg("请求服务器发生异常，请联系系统管理员或者稍后再试!");
                    }
                });
            }
        });

        form.on('submit(submit)', function (data) {
            data.field.TagID = data.field.ArticleTag;
            data.field.TagName = $('.articleTag').find("option:selected").text();
            data.field.HtmlContent = editor.getHTML();
            data.field.MarkDownContent = editor.getMarkdown();
            data.field.ID = $("#editor").data("article-id");

            GetDataByJson($(this).data("request-url"), 'post', data.field, function (data) {
                if (data.IsSuccess) {
                    layer.msg(data.Message);
                    location.href = $(".button-submit").data("article-url") + "?articleID=" + data.Data;
                } else {
                    layer.msg(data.Message);
                }

            });
        });
    })
});