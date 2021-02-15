$(function(){
    var editor = editormd("editor",{ //id,后续name要开头一致
        width:"100%",
        height:800,
        syncScrolling:"single",
        path:"../editor.md/lib/",  //插件路径
        saveHTMLToTextarea:true,  //影响第二个text能否获取html内容
        emoji:true,  //开启emoji
        flowChart: true,//开启流程图支持
        sequenceDiagram: true,//开启时序/序列图支持
    });

    layui.use(['jquery','form','layer'], function () {
    var $ = layui.jquery;
    var form = layui.form;
    var layer = layui.layer;

    form.on('submit(submit)',function(data){
        data.field.TagID = data.field.ArticleTag;
        data.field.TagName = $('.articleTag').find("option:selected").text();
        data.field.HtmlContent = editor.getHTML();
        data.field.MarkDownContent = editor.getMarkdown();

        GetDataByJson($(this).data("request-url"),'post',data.field,function(data){
            if(data.IsSuccess){
                layer.msg(data.Message);
                //location.href = $(".button-submit").data("article-url") + "?articleID=" + data.Data;
                console.log(data.Data);
            } else {
                layer.msg(data.Message);
            }

        });
    });





});
});