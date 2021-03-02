/*

@Name：不落阁整站模板源码 
@Author：Absolutely 
@Site：http://www.lyblogs.cn

*/


$(function () {
    var editor = editormd.markdownToHTML("editor-content", {//注意：这里是上面DIV的id
            htmlDecode: "style,script,iframe",
            emoji: true,
            taskList: true,
            tex: true, // 默认不解析
            flowChart: true, // 默认不解析
            sequenceDiagram: true, // 默认不解析
            codeFold: true,
    });

    //导航栏选中
    $("#nav-text").addClass("layui-this").siblings().removeClass("layui-this");
    sessionStorage.setItem("selected", $("#nav-text").find('a').attr('href'));
});

layui.use(['form', 'layedit'], function () {
    var form = layui.form;
    var $ = layui.jquery;
    var layedit = layui.layedit;

    $('.article-delete').click(function () {
        layer.confirm('确认删除文章？', { icon: 3, title: '删除文章' }, function (index) {
            GetDataByJson($(".article-delete").data("request-url"), "post", null, function (data) {
                if (data.IsSuccess) {
                    layer.msg(data.Message);
                    location.href = $(".article-delete").data("home-url");
                }
                else {
                    layer.msg(data.Message)
                }
            });
        });
    });

    //评论和留言的编辑器
    var editIndex = layedit.build('remarkEditor', {
        height: 150,
        tool: ['face', '|', 'left', 'center', 'right', '|', 'link'],
    });
    //评论和留言的编辑器的验证
    layui.form.verify({
        content: function (value) {
            value = $.trim(layedit.getText(editIndex));
            if (value == "") return "自少得有一个字吧";
            layedit.sync(editIndex);
        }
    });

    //监听评论提交
    form.on('submit(formRemark)', function (data) {
        var index = layer.load(1);
        //模拟评论提交
        setTimeout(function () {
            layer.close(index);
            var content = data.field.editorContent;
            var html = '<li><div class="comment-parent"><img src="../images/Absolutely.jpg"alt="absolutely"/><div class="info"><span class="username">Absolutely</span><span class="time">2017-03-18 18:46:06</span></div><div class="content">' + content + '</div></div></li>';
            $('.blog-comment').append(html);
            $('#remarkEditor').val('');
            editIndex = layui.layedit.build('remarkEditor', {
                height: 150,
                tool: ['face', '|', 'left', 'center', 'right', '|', 'link'],
            });
            layer.msg("评论成功", { icon: 1 });
        }, 500);
        return false;
    });
});