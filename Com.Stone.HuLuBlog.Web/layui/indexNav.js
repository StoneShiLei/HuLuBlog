$(function () {

  
  // function getMenus() {
  //   $(".left-nav").html(a)
  //   element.init();
  // }
  var element
  // var pageFunctions = {
  // getMenus() {
  // $.get('http://10.168.1.247/CMEBTWebApi/Menu/GetUserMenu', function (res) {
  //   var data = JSON.parse(res).rows
  //   data.forEach(function name(i) {
  //     var childStr = ''
  //     i.children.forEach(function (j) {
  //       childStr += '<li date-refresh="1"><a _href="member-list.html"><i class="iconfont">&#xe6a7;</i><cite>' + j.title + '</cite></a></li>'
  //     })
  //     var htmlStr =
  //       // '<li><a href="javascript:;"><i class="iconfont">&#xe6b8;</i><cite>' + i.title +
  //       // '</cite><i class="iconfontnav_right">></i></a><ul>' + childStr + '</ul></li>'
  //       '  <li class="layui-nav-item layui-nav-itemed"><a href="javascript:;">默认展开</a><dl class="layui-nav-child"><dd><a href="javascript:;">选项1</a></dd><dd><a href="javascript:;">选项2</a></dd><dd><a href="">跳转</a></dd></dl></li>'
  //   });
  // })
  // }
  // }
  var a = `
                <ul id="nav">
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe6b8;</i>
                        <cite>会员管理</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li date-refresh="1">
                            <a _href="member-list.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>会员列表(静态表格)</cite>
                                
                            </a>
                        </li >
                        <li>
                            <a _href="member-list1.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>会员列表(动态表格)</cite>
                                
                            </a>
                        </li >
                        <li date-refresh="1">
                            <a _href="member-del.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>会员删除</cite>
                                
                            </a>
                        </li>
                        <li>
                            <a href="javascript:;">
                                <i class="iconfont">&#xe70b;</i>
                                <cite>会员管理</cite>
                                <i class="iconfont nav_right">&#xe697;</i>
                            </a>
                            <ul class="sub-menu">
                                <li>
                                    <a _href="xxx.html">
                                        <i class="iconfont">&#xe6a7;</i>
                                        <cite>会员列表</cite>
                                        
                                    </a>
                                </li >
                                <li>
                                    <a _href="xx.html">
                                        <i class="iconfont">&#xe6a7;</i>
                                        <cite>会员删除</cite>
                                        
                                    </a>
                                </li>
                                <li>
                                    <a _href="xx.html">
                                        <i class="iconfont">&#xe6a7;</i>
                                        <cite>等级管理</cite>
                                        
                                    </a>
                                </li>
                                
                            </ul>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe723;</i>
                        <cite>订单管理</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a _href="order-list.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>订单列表</cite>
                            </a>
                        </li >
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe723;</i>
                        <cite>分类管理</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a _href="cate.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>多级分类</cite>
                            </a>
                        </li >
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe723;</i>
                        <cite>城市联动</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a _href="city.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>三级地区联动</cite>
                            </a>
                        </li >
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe726;</i>
                        <cite>管理员管理</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a _href="admin-list.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>管理员列表</cite>
                            </a>
                        </li >
                        <li>
                            <a _href="admin-role.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>角色管理</cite>
                            </a>
                        </li >
                        <li>
                            <a _href="admin-cate.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>权限分类</cite>
                            </a>
                        </li >
                        <li>
                            <a _href="admin-rule.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>权限管理</cite>
                            </a>
                        </li >
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe6ce;</i>
                        <cite>系统统计</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a _href="echarts1.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>拆线图</cite>
                            </a>
                        </li >
                        <li>
                            <a _href="echarts2.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>柱状图</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="echarts3.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>地图</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="echarts4.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>饼图</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="echarts5.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>雷达图</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="echarts6.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>k线图</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="echarts7.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>热力图</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="echarts8.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>仪表图</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe6b4;</i>
                        <cite>图标字体</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a _href="unicode.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>图标对应字体</cite>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href="javascript:;">
                        <i class="iconfont">&#xe6b4;</i>
                        <cite>其它页面</cite>
                        <i class="iconfont nav_right">&#xe697;</i>
                    </a>
                    <ul class="sub-menu">
                        <li>
                            <a href="login.html" target="_blank">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>登录页面</cite>
                            </a>
                        </li>
                        <li>
                            <a _href="error.html">
                                <i class="iconfont">&#xe6a7;</i>
                                <cite>错误页面</cite>
                            </a>
                        </li>
                    </ul>
                </li>
            </ul>`
  // $('.left-nav').append(a)


  var b = {
    "success": true,
    "msg": "",
    "total": 4,
    "rows": [{
        "spread": false,
        "title": "通知公告",
        "href": "",
        "icon": "",
        "children": [{
            "spread": false,
            "title": "发布公告",
            "href": "/CMEBTWeb/Notice/NoticeAdd.html",
            "icon": "",
            "children": null
          },
          {
            "spread": false,
            "title": "公告列表",
            "href": "/CMEBTWeb/Notice/NoticeAllList.html",
            "icon": "",
            "children": null
          },
          {
            "spread": false,
            "title": "我发布的公告",
            "href": "/CMEBTWeb/Notice/MyNoticeAllList.html",
            "icon": "",
            "children": null
          },
          {
            "spread": false,
            "title": "个人收藏",
            "href": "/CMEBTWeb/Notice/CollectNoticeAllList.html",
            "icon": "",
            "children": null
          }
        ]
      },
      {
        "spread": false,
        "title": "网点查询",
        "href": "",
        "icon": "",
        "children": [{
            "spread": false,
            "title": "乡镇工作人员信息",
            "href": "/CMEBTWeb/NetHelper/WorkManInfo.html",
            "icon": "",
            "children": null
          },
          {
            "spread": false,
            "title": "就业援助员信息",
            "href": "/CMEBTWeb/NetHelper/NetWorkWorkHelperInfoList.html",
            "icon": "",
            "children": null
          }
        ]
      },
      {
        "spread": false,
        "title": "政策文件",
        "href": "",
        "icon": "",
        "children": [{
            "spread": false,
            "title": "政策文件新增",
            "href": "/CMEBTWeb/Document/DocumentAdd.html",
            "icon": "",
            "children": null
          },
          {
            "spread": false,
            "title": "政策文件管理",
            "href": "/CMEBTWeb/Document/DocumentAllList.html",
            "icon": "",
            "children": null
          },
          {
            "spread": false,
            "title": "政策文件列表",
            "href": "",
            "icon": "",
            "children": null
          }
        ]
      },
      {
        "spread": false,
        "title": "系统管理",
        "href": "",
        "icon": "",
        "children": [{
            "spread": false,
            "title": "站点设置",
            "href": "",
            "icon": "",
            "children": [{
                "spread": false,
                "title": "站点列表",
                "href": "SsoMana/Site/List.aspx",
                "icon": "",
                "children": null
              },
              {
                "spread": false,
                "title": "参数设置",
                "href": "SsoMana/Config/List.aspx",
                "icon": "",
                "children": null
              }
            ]
          },
          {
            "spread": false,
            "title": "组织架构",
            "href": "",
            "icon": "",
            "children": [{
                "spread": false,
                "title": "单位管理",
                "href": "SystemMana/Org/OrgFrame.aspx",
                "icon": "",
                "children": null
              },
              {
                "spread": false,
                "title": "部门管理",
                "href": "SystemMana/Dept/DeptFrame.aspx",
                "icon": "",
                "children": null
              },
              {
                "spread": false,
                "title": "用户管理",
                "href": "SystemMana/User/UserFrame.aspx",
                "icon": "",
                "children": null
              },
              {
                "spread": false,
                "title": "角色管理",
                "href": "SystemMana/Role/RoleFrame.aspx",
                "icon": "",
                "children": null
              },
              {
                "spread": false,
                "title": "系统菜单",
                "href": "SystemMana/Module/ModuleFrame.aspx",
                "icon": "",
                "children": null
              }
            ]
          },
          {
            "spread": false,
            "title": "代码管理",
            "href": "",
            "icon": "",
            "children": [{
                "spread": false,
                "title": "代码列表",
                "href": "SystemMana/Code/CodeFrame.aspx",
                "icon": "",
                "children": null
              },
              {
                "spread": false,
                "title": "数据字典",
                "href": "SystemMana/CodeDictionary/CodeFrame.aspx",
                "icon": "",
                "children": null
              }
            ]
          },
          {
            "spread": false,
            "title": "在线用户管理",
            "href": "",
            "icon": "",
            "children": [{
              "spread": false,
              "title": "在线用户列表",
              "href": "SystemMana/OnlineUser/List.aspx",
              "icon": "",
              "children": null
            }]
          }
        ]
      }
    ]
  }
  // layui.use('element', function () {
  //   element = layui.element;
  //   $(".left-nav").html(a)
  //   element.init();
  //   element.render('nav','test1');
  //   console.dir(element.render)
  //   // pageFunctions.getMenus()
  //   // getMenus()
  // });
})