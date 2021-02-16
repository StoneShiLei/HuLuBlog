$(function () {
    layui.use(['jquery','table', 'layer'], function () {
        var $ = layui.jquery;
        var form = layui.form;
        var layer = layui.layer;
        var table = layui.table;
        var url = $(".table-container").data("request-url");

        table.render({
            id:'tagTable',
            elem: '#tag-table',
            url: url,
            toolbar: '#toolbarhead',
            defaultToolbar:[],
            parseData: function (res) {
                return {
                    "code": res.Data.code,
                    "msg": res.Message,
                    "count": res.Data.count,
                    "data": res.Data.data
                }
            },
            cols: [[
                {field:'ID',title:'ID',hide:true},
                {field:'TagName',title:'标签名',minwidth:200,edit:'text'},
                {field:'ArticleCount',title:'文章数',align:'center',width:80},
                { fixed: 'right',title:'操作', align:'center',toolbar:'#toolbar' },
            ]]
        });

        //监听单元格修改
        table.on('edit(tagTable)', function (obj) {
            var value = obj.value; //修改后的值
            var data = obj.data; //所在行所有键值
            GetDataByJson($(".table-container").data("tagrename-url"), "post",
                { TagName: value, ID: data.ID }, function (data) {
                    if (data.IsSuccess) {
                        layer.msg(data.Message);
                    }
                    else {
                        layer.msg(data.Message)
                    }
            });
        });

        //监听行工具事件
        table.on('tool(tagTable)', function (obj) {
            var data = obj.data;
            if (obj.event === 'delete') {
                layer.confirm('确定删除标签？', function (index) {
                    GetDataByJson($(".table-container").data("tagdelete-url"), "post", {
                        ID: data.ID,
                        TagName:data.TagName
                    }, function (data) {
                        if (data.IsSuccess) {
                            //关闭该 confirm 窗口
                            layer.close(index);
                            layer.msg(data.Message);
                            table.reload('tagTable');
                        }
                        else {
                            layer.close(index);
                            layer.msg(data.Message)
                            table.reload('tagTable');
                        }
                    });
                });
            }
        });

        //头工具栏事件
        table.on('toolbar(tagTable)', function (obj) {
            switch (obj.event) {
                case 'add':
                    GetDataByJson($(".table-container").data("tagadd-url"), "post",
                        { TagName: $(".add-tagname").val(), ID: "1" }, function (data) {
                            if (data.IsSuccess) {
                                layer.msg(data.Message);
                                //$(".add-tagname").val() = "";
                                table.reload('tagTable');
                            }
                            else {
                                layer.msg(data.Message)
                            }
                    })
                    break;
            };
        });

        //恢复文章按钮
        $('.article-recover').click(function () {
            GetDataByJson($(this).data("request-url"), "post", {
                articleID : $(this).data("article-id")
            }, function (data) {
                if (data.IsSuccess) {
                    layer.msg(data.Message);
                    location.reload();
                }
                else {
                    layer.msg(data.Message)
                }
            });
        });

        //删除文章按钮
        $('.article-delete').click(function () {
            GetDataByJson($(this).data("request-url"), "post", {
                articleID : $(this).data("article-id")
            }, function (data) {
                if (data.IsSuccess) {
                    layer.msg(data.Message);
                    location.reload();
                }
                else {
                    layer.msg(data.Message)
                }
            });
        });

        ////删除标签按钮
        //$('.tag-delete').click(function () {
        //    GetDataByJson($(this).data("request-url"), "post", {
        //        ID: $(this).data("tag-id"),
        //        TagName:$(this).data("tag-name")
        //    }, function (data) {
        //        if (data.IsSuccess) {
        //            layer.msg(data.Message);
        //            location.reload();
        //        }
        //        else {
        //            layer.msg(data.Message)
        //        }
        //    });
        //});

    })
});