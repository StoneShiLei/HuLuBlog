/*

@Name：不落阁整站模板源码 
@Author：Absolutely 
@Site：http://www.lyblogs.cn

*/

layui.use(['jquery','layer'], function () {
    var $ = layui.jquery;
    var layer = layui.layer;

    $(function () {
        //播放公告
        playAnnouncement(3000);
    });
    function playAnnouncement(interval) {
        var index = 0;
        var $announcement = $('.home-tips-container>span');
        //自动轮换
        setInterval(function () {
            index++;    //下标更新
            if (index >= $announcement.length) {
                index = 0;
            }
            $announcement.eq(index).stop(true, true).fadeIn().siblings('span').fadeOut();  //下标对应的图片显示，同辈元素隐藏
        }, interval);
    };

    $('.login-buttom').click(function(){
        layer.open({
            type:2,
            title:"请登录",
            shadeClose:true,
            area:['380px','270px'],
            content: $(this).data("request-url"),
            resize: false,
            move:false
        })
    });

    $('#email').click(function () {
        layer.open({
            type: 2,
            title: "发送邮件",
            shadeClose: false,
            area: ['500px', '360px'],
            resize: true,
            move: true,
            content:$(this).data("request-url")
        })

    });





});
