/*

@Name：不落阁整站模板源码 
@Author：Absolutely 
@Site：http://www.lyblogs.cn

*/

layui.use(['element', 'layer', 'util', 'form'], function () {
    var $ = layui.jquery;

    //回到顶部
    layui.util.fixbar({
        css: {right:75,bottom:100}
    });

    //导航菜单选中效果  //todo 暂定
    if (sessionStorage.getItem("selected")) {
        $(".layui-nav-item").each(function () {
            if ($(this).find('a').attr('href') == sessionStorage.getItem("selected")) {
                $(this).addClass("layui-this").siblings().removeClass("layui-this");
            }
        })
    }
    else {
        $(".layui-nav-item").eq(0).addClass('layui-this');
    }
    $('.layui-nav-item').click(function () {
        $(this).addClass("layui-this").siblings().removeClass("layui-this");
        sessionStorage.setItem("selected", $(this).find('a').attr('href'));
    });

    //子栏目导航点击事件
    $('.child-nav span').click(function () {
        layer.msg('切换到相应栏目');
        $(this).addClass('child-nav-btn-this').siblings().removeClass('child-nav-btn-this');
    });

    //侧边导航开关点击事件
    $('.blog-navicon').click(function () {
        var sear = new RegExp('layui-hide');
        if (sear.test($('.blog-nav-left').attr('class'))) {
            leftIn();
        } else {
            leftOut();
        }
    });
    //侧边导航遮罩点击事件
    $('.blog-mask').click(function () {
        leftOut();
    });
    //类别导航开关点击事件
    $('.category-toggle').click(function (e) {
        e.stopPropagation();    //阻止事件冒泡
        categroyIn();
    });
    //类别导航点击事件，用来关闭类别导航
    $('.article-category').click(function () {
        categoryOut();
    });
    //具体类别点击事件
    $('.article-category > a').click(function (e) {
        e.stopPropagation(); //阻止事件冒泡
    });


    //显示侧边导航
    function leftIn() {
        $('.blog-mask').unbind('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend');
        $('.blog-nav-left').unbind('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend');

        $('.blog-mask').removeClass('maskOut');
        $('.blog-mask').addClass('maskIn');
        $('.blog-mask').removeClass('layui-hide');
        $('.blog-mask').addClass('layui-show');

        $('.blog-nav-left').removeClass('leftOut');
        $('.blog-nav-left').addClass('leftIn');
        $('.blog-nav-left').removeClass('layui-hide');
        $('.blog-nav-left').addClass('layui-show');
    }
    //隐藏侧边导航
    function leftOut() {
        $('.blog-mask').on('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $('.blog-mask').addClass('layui-hide');
        });
        $('.blog-nav-left').on('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $('.blog-nav-left').addClass('layui-hide');
        });

        $('.blog-mask').removeClass('maskIn');
        $('.blog-mask').addClass('maskOut');
        $('.blog-mask').removeClass('layui-show');

        $('.blog-nav-left').removeClass('leftIn');
        $('.blog-nav-left').addClass('leftOut');
        $('.blog-nav-left').removeClass('layui-show');
    }
    //显示类别导航
    function categroyIn() {
        $('.category-toggle').addClass('layui-hide');
        $('.article-category').unbind('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend');

        $('.article-category').removeClass('categoryOut');
        $('.article-category').addClass('categoryIn');
        $('.article-category').addClass('layui-show');
    }
    //隐藏类别导航
    function categoryOut() {
        $('.article-category').on('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $('.article-category').removeClass('layui-show');
            $('.category-toggle').removeClass('layui-hide');
        });

        $('.article-category').removeClass('categoryIn');
        $('.article-category').addClass('categoryOut');
    }

});
