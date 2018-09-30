(function ($) {
    //上拉的标示
    var upFlag;
    //下拉的标示
    var downFlag;
    var _callbackDownEx;
    var _callbackUpEx;
    $.mPage = {
        //textObjs容器id 循环列表的父容器Id,callback是拼html的回调函数,getData是获取数据的函数,pgObjs是当前页码,sizes总的页数,绝对定位距离页面顶部高度,绝对定位距离页面顶部宽度,该控件是绝对定位需自行制定距离页面顶部的高度距离页面底部的高度
        //extraId是需要保留结构的最上层的Id
        bPage: function (jsonParams) {
            //处理参数
            var textObjId = jsonParams.textObjId;
            var callback = jsonParams.callback;
            var getData = jsonParams.getData;
            var pgObjs = jsonParams.pgObjs;
            var sizes = jsonParams.sizes;
            var top = jsonParams.top;
            var bottom = jsonParams.bottom;
            var extraId = jsonParams.extraId;
            var endHtml = jsonParams.endHtml;
            var noDataHtml = jsonParams.noDataHtml;
            if (!noDataHtml) {
                noDataHtml = endHtml;
            }
            var refreshDownHtml = '<div class="refresh-con"><p class="txt gray-f"><span class="bg an1"><img src="../Theme/plugins/Pager/img/refresh.png" width="25px"></span>刷新内容</p></div>';
            var refreshRollHtml = '<div class="refresh-con"><p class="txt gray-f"><span class="bg an2 "><img src="../Theme/plugins/Pager/img/Shape.png" width="20px"></span>刷新内容</p></div>';
            var refreshUpHtml = '<div class="refresh-bot"><div class="rect1"></div><div class="rect2"></div><div class="rect3"></div><div class="rect4"></div></div>';
            textObjs = (typeof textObjId == 'string') ? $('#' + textObjId) : textObjId;
            var objHtml = "";
            var extraObjs;
            var thisObjs;
            if (extraId) {
                //获取元素自身的html
                objHtml = $("#" + extraId).prop("outerHTML");
                extraObjs = $("#" + extraId);
            }
            else {
                //获取元素自身的html
                objHtml = textObjs.prop("outerHTML");
            }

            //如果存在额外的容器
            if (extraObjs) {
                thisObjs = extraObjs;
            } else {
                thisObjs = textObjs;
            }
            //每次加载需先删除结尾元素
            if (thisObjs.next().attr("stype") && thisObjs.next().attr("stype") === "end") {
                thisObjs.next().remove();
            }
            if (pgObjs != 1) {
                //回调上拉
                _callbackUpEx();
                if (pgObjs == sizes) {
                    $.mPage.unbUpPage();
                    $.mPage.setEndHtml(textObjId, extraId, endHtml);
                }
                return;
            }
            //为第一页的情况
            //下拉刷新的话 重新设置是否只有一页的状态
            if ($("#wrapper").length > 0) {
                if (sizes <= 1) {
                    $("#pullUp").hide();
                    upFlag = true;
                    if (sizes == 0) {
                        //只有一页的时候显示没有数据,并且样式上将此页铺满 type传1是结尾页高度为warpper高度无需先设置数据
                        $.mPage.setEndHtml(textObjId, extraId, noDataHtml, 1);
                    } else {
                        //只有一页的时候显示没有数据,并且样式上将此页铺满 
                        //这里必须先加载数据再设置结尾页 否则结尾页的高度无法设置
                        _callbackDownEx();
                        $.mPage.setEndHtml(textObjId, extraId, endHtml, 2);
                        return;
                    }
                } else {
                    $("#pullUp").show();
                    upFlag = false;
                }
                if (top || top === 0) {
                    $("#wrapper").css("top", top + "px");
                }
                if (bottom || bottom === 0) {
                    $("#wrapper").css("bottom", bottom + "px");
                }
                //回调下拉 先加载数据
                _callbackDownEx();
                return;
            } else {
                //拼接html并且插入到该元素的后面
                var html = '<div id="wrapper">' +
                    '<div id="scroller">';
                html += '<div id="pullDown">' +
                    '<span class="pullDownLabel">' + refreshDownHtml + '</span>' +
                    '</div>';
                html += objHtml;
                html += '<div id="pullUp">' +
                    '<div class="pullUpLabel">' + refreshUpHtml + '</div>' +
                    '</div></div></div>';
                var _wrapper = $(html);
                thisObjs.after(_wrapper);
                thisObjs.remove();
                if (top || top === 0) {
                    $("#wrapper").css("top", top + "px");
                }
                if (bottom || bottom === 0) {
                    $("#wrapper").css("bottom", bottom + "px");
                }
                //第一次走上拉拼接逻辑
                textObjs = (typeof textObjId == 'string') ? $('#' + textObjId) : textObjId;
                var json = callback();
                textObjs.html(json.html ? json.html : json);
                if (json.event) {
                    json.event();
                }
                upFlag = false;
                if (sizes == 1) {
                    $.mPage.setEndHtml(textObjId, extraId, endHtml, 2);
                }
                if (sizes == 0) {
                    $.mPage.setEndHtml(textObjId, extraId, noDataHtml, 1);
                }
            }
            _callbackDownEx = function () {
                var json = callback();
                if (!json.html) {
                    textObjs.html(json);
                } else {
                    textObjs.html(json.html);
                    json.event();
                }
                myScroll.refresh();
            }
            _callbackUpEx = function () {
                var json = callback();
                if (!json.html) {
                    textObjs.append(json);
                } else {
                    textObjs.append(json.html);
                    json.event();
                }
                upFlag = false;
                myScroll.refresh();
            }
            var _callbackDown = function () {
                getData("down");
            }
            var _callbackUp = function () {
                getData("up");
            }
            setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 800);
            //当页数为1的时候 就没有上拉了
            var myScroll;
            var pullDownEl = document.getElementById('pullDown');
            var pullDownOffset = pullDownEl.offsetHeight;
            var pullUpEl = document.getElementById('pullUp');
            var pullUpOffset = pullUpEl.offsetHeight;
            downFlag = false;
            upFlag = false;
            //只有一页的时候无法上拉，页数读完无法上拉
            if (sizes <= 1) {
                $("#pullUp").hide();
                upFlag = true;
            } else {
                $("#pullUp").show();
                upFlag = false;
            }
            var hg = 5;

            myScroll = new iScroll('wrapper', {
                scrollbarClass: 'myScrollbar',
                useTransition: false,
                minScrollY: 0,
                minScrollX: 0,
                topOffset: pullDownOffset,
                onRefresh: function () {
                    if (pullDownEl.className.match('loading')) {
                        pullDownEl.className = '';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = refreshDownHtml;
                    } else if (pullUpEl.className.match('loading')) {
                        pullUpEl.className = '';
                        pullUpEl.querySelector('.pullUpLabel').innerHTML = refreshUpHtml;
                    }
                },
                lockDirection: true,
                onScrollMove: function () {
                    if (this.y > hg && !pullDownEl.className.match('flip')) {
                        pullDownEl.className = 'flip';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = refreshDownHtml;
                        this.minScrollY = 0;
                    } else if (this.y < hg && pullDownEl.className.match('flip')) {
                        pullDownEl.className = '';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = refreshDownHtml;
                        this.minScrollY = -pullDownOffset;
                    } else if (this.y < (this.maxScrollY - hg) && !pullUpEl.className.match('flip')) {
                        if (upFlag == true) {
                            return;
                        }
                        pullUpEl.className = 'flip';
                        pullUpEl.querySelector('.pullUpLabel').innerHTML = refreshUpHtml;
                        this.maxScrollY = this.maxScrollY;
                    } else if (this.y > (this.maxScrollY + hg) && pullUpEl.className.match('flip')) {
                        if (upFlag == true) {
                            return;
                        }
                        pullUpEl.className = '';
                        pullUpEl.querySelector('.pullUpLabel').innerHTML = refreshUpHtml;
                        this.maxScrollY = pullUpOffset;
                    }
                },
                onScrollEnd: function () {
                    if (pullDownEl.className.match('flip')) {
                        if (downFlag == true) {
                            return;
                        }
                        pullDownEl.className = 'loading';
                        pullDownEl.querySelector('.pullDownLabel').innerHTML = refreshRollHtml;
                        _callbackDown();
                    } else if (pullUpEl.className.match('flip')) {
                        if (upFlag == true) {
                            return;
                        }
                        upFlag = true;
                        pullUpEl.className = 'loading';
                        pullUpEl.querySelector('.pullUpLabel').innerHTML = refreshUpHtml;
                        _callbackUp();
                    }
                }
            });


        },
        unbUpPage: function () {
            $("#pullUp").hide();
            upFlag = true;
        },
        bUpPage: function () {
            $("#pullUp").show();
            upFlag = false;
        },
        //设置宽高
        setRange: function (top, bottom) {
            if ($("#wrapper").length > 0) {
                if (top || top === 0) {
                    $("#wrapper").css("top", top + "px");
                }
                if (bottom || bottom === 0) {
                    $("#wrapper").css("bottom", bottom + "px");
                }
            }

        },
        //1代表结尾
        setEndHtml: function (textObjId, extraId, html, type) {
            var objs;
            if (html) {
                var dom = $(html)
                if (extraId) {
                    $("#" + extraId).after(dom);
                } else {
                    $("#" + textObjId).after(dom);
                }
                dom.attr("stype", "end");
                if (type && type == 1) {
                    var panelHeight = $("#wrapper").get(0).offsetHeight;
                    var marginTop = parseInt($("div [stype=end]").css("margin-top"));
                    var marginBottom = parseInt($("div [stype=end]").css("margin-bottom"));
                    var domHeight = parseInt(dom.get(0).offsetHeight);
                    var height = panelHeight - marginTop - marginBottom;
                    dom.css("height", height);
                }
                if (type && type == 2) {
                    var panelHeight = $("#wrapper").get(0).offsetHeight;
                    var marginTop = parseInt($("div [stype=end]").css("margin-top"));
                    var marginBottom = parseInt($("div [stype=end]").css("margin-bottom"));
                    var chaHeight = dom.prev().get(0).offsetHeight;
                    var height = panelHeight - marginTop - marginBottom - chaHeight;
                    if (height <= 0) {
                        return;
                    }
                    dom.css("height", height);
                }
            }
        }
    }
})($);

$(function () {
    /// <summary>
    /// 页面头部返回事件
    /// </summary>
    /// <returns></returns>
    /// <remarks>2015-12-24 何成 创建</remarks>
    $(".go-back").bind("click", function () {
        history.back();
    });
});
