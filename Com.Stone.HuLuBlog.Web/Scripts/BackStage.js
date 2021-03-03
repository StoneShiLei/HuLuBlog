$(function () {
    layui.use(['jquery','table', 'layer'], function () {
        var $ = layui.jquery;
        var form = layui.form;
        var layer = layui.layer;
        var table = layui.table;
        var tagUrl = $(".table-container").data("request-url");
        var friendUrl = $(".friendlink-table-container").data("request-url");

        //tag表格
        table.render({
            id:'tagTable',
            elem: '#tag-table',
            url: tagUrl,
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

        //监听tag单元格修改
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

        //监听tag行工具事件
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

        //tag头工具栏事件
        table.on('toolbar(tagTable)', function (obj) {
            switch (obj.event) {
                case 'add':
                    GetDataByJson($(".table-container").data("tagadd-url"), "post",
                        { TagName: $(".add-tagname").val(), ID: "1" }, function (data) {
                            if (data.IsSuccess) {
                                layer.msg(data.Message);
                                table.reload('tagTable');
                            }
                            else {
                                layer.msg(data.Message)
                            }
                    })
                    break;
            };
        });

        //friendlink表格
        table.render({
            id: 'friendlinkTable',
            elem: '#friendlink-table',
            url: friendUrl,
            toolbar: '#friendlink-toolbarhead',
            defaultToolbar: [],
            parseData: function (res) {
                return {
                    "code": res.Data.code,
                    "msg": res.Message,
                    "count": res.Data.count,
                    "data": res.Data.data
                }
            },
            cols: [[
                { field: 'ID', title: 'ID', hide: true },
                { field: 'Title', title: '标题', width: 120, edit: 'text' },
                { field: 'IconUrl', title: '图标', align: 'center', width: 80, edit: 'text' },
                { field: 'Domain', title: '域名', align: 'center', width: 80, edit: 'text'},
                { field: 'Url', title: '链接', align: 'center', width: 80, edit: 'text' },
                { fixed: 'right', title: '操作', align: 'center', toolbar: '#friendlink-toolbar' },
            ]]
        });

        //监听friendlink行工具事件
        table.on('tool(friendlinkTable)', function (obj) {
            var data = obj.data;
            if (obj.event === 'delete') {
                layer.confirm('确定删除友链？', function (index) {
                    GetDataByJson($(".friendlink-table-container").data("friendlink-delete-url"), "post", {
                        ID: data.ID,
                        Title: data.Title,
                        IconUrl: data.IconUrl,
                        Domain: data.Domain,
                        Url: data.Url
                    }, function (data) {
                        if (data.IsSuccess) {
                            //关闭该 confirm 窗口
                            layer.close(index);
                            layer.msg(data.Message);
                            table.reload('friendlinkTable');
                        }
                        else {
                            layer.close(index);
                            layer.msg(data.Message)
                            table.reload('friendlinkTable');
                        }
                    });
                });
            }
        });

        //friendlink头工具栏事件
        table.on('toolbar(friendlinkTable)', function (obj) {
            switch (obj.event) {
                case 'add':
                    var data = {};
                    data.title = $(".friendlink-title").val();
                    data.iconUrl = $(".friendlink-iconUrl").val();
                    data.domain = $(".friendlink-domain").val();
                    data.url = $(".friendlink-url").val();
                    data.id = "1";
                    GetDataByJson($(".friendlink-table-container").data("friendlink-add-url"), "post",
                        data, function (data) {
                            if (data.IsSuccess) {
                                layer.msg(data.Message);
                                table.reload('friendlinkTable');
                            }
                            else {
                                layer.msg(data.Message)
                            }
                        });
                    break;
            };
        });

        //监听friendlink单元格修改
        table.on('edit(friendlinkTable)', function (obj) {
            var value = obj.value; //修改后的值
            var data = obj.data; //所在行所有键值
            GetDataByJson($(".friendlink-table-container").data("friendlink-rename-url"), "post",
                data, function (data) {
                    if (data.IsSuccess) {
                        layer.msg(data.Message);
                    }
                    else {
                        layer.msg(data.Message)
                    }
                });
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

        //更新索引
        $('.update-index').click(function () {
            GetDataByJson($(this).data("request-url"), "post", null, function (data) {
                if (data.IsSuccess) {
                    layer.msg(data.Message);
                }
                else {
                    layer.msg(data.Message);
                }
            });
        });

    })
});