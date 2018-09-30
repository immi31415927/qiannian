UI = UI || {};


/* 
* 系统分页定制插件
*/
UI.WebPager = function (selector, options) {

    var defaults = {
        totalCount: 0, //总条数
        totalPages: 0, //总页数
        pageSize: 10, //每页显示数
        currentPage: 1, //当前页
        numericPagerItemCount: 8, //每页分页按钮显示数
        additionTotal: 10, //每页显示条数中生成的页码增量(20, 30, 40)
        holder: null,
        templateId: null, //当类型为json时必须设置模板Id
        afterBindDataEvent: null,
        lang: { //语言
            bar: '条',
            previousPage: '«',
            nextPage: '»',
            displayPerPage: '每页显示',
            noDataDisplay: '没有数据',
            falseDataNote: '网络是否通畅'
        },
        searchFormID: 'formSearch',
        ajax: //数据源设置 
        {
            url: null,
            method: 'GET',
            data: {}, //请求参数数据
            dataType: 'html' //当类型为json时必须设置模板Id,html不用设置
        }
    };

    var options = $.extend(true, {}, defaults, options);

    //对象
    selector = $(selector); //一页最少显示的条数，以这个为基数以5向上增加三个
    //var basicPageSize = options.pageSize;

    ////创建页大小选择控件
    //function buildPageSizeSelector(pageSize) {
    //    pageSize = parseInt(pageSize);
    //    var sizeArr = [10, 20, 50, 100, 150, 200];
    //    if (sizeArr.indexOf(pageSize) < 0) {
    //        sizeArr.push(pageSize);
    //    }
    //    var str = "<select class='input-page-size wd60' style='border:1px solid #ccc'>";
    //    for (var i = 0; i < sizeArr.length; i++) {
    //        str += "<option" + (sizeArr[i] === pageSize ? " selected='selected'" : "") + ">" + sizeArr[i] + "</option>";
    //    }
    //    str += "</select>";
    //    return str;
    //}

    //绑定分页事件
    function bindEvent() {
        var that = selector;

        //绑定a标签点击事件
        that.each(function () {
            that.next().find("a").click(function () {
                if (!$(this).parent().hasClass("am-disabled")) {
                    var clickedLink = $(this).attr("rel");
                    options.currentPage = parseInt(clickedLink);
                    bindData();
                    return false;
                }
            });
        });
        selector.next().find("li.am-active a").unbind('click');

        /*处理上一页和下一页事件*/
        if (options.currentPage <= 1) {
            selector.next("li:first a").unbind("click");
        }
        if (options.currentPage >= options.totalPages) {
            selector.next("li:last a").unbind("click");
        }

        return that;
    }
    //绑定数据
    function bindData() {
        // var cover = UI.Mask({ obj: selector, opacity: 0.2 });
        UI.Loading();

        options.ajax.data.CurrentPageIndex = options.currentPage;
        options.ajax.data.pageSize = options.pageSize;
        var targetData, result = {};
        if (options.ajax.url) {
            $.ajax({
                type: options.ajax.method || options.ajax.type,
                url: options.ajax.url,
                data: $.param(options.ajax.data) + "&" + $("#" + options.searchFormID).serialize(),
                //async: false,
                success: function (data) {
                    if (data.Status != undefined && !data.Status) {
                        //Ims.UI.tip_alert('alert-danger', data.Message);
                        // cover.Remove();
                        UI.Hide();
                    } else {
                        result = data;

                        if (options.ajax.dataType === 'json') {
                            targetData = template(options.templateId, result);
                        } else if (options.ajax.dataType === 'html') {
                            targetData = result;
                        }

                        selector.html(targetData);

                        //取每页显示数
                        var pageSize = options.pageSize; // parseInt(selector.find("#PageSize").val());
                        if (pageSize) {
                            options.pageSize = pageSize;
                        }
                        //取分页总数
                        var totalCount = parseInt(selector.find("#TotalCount").val());
                        if (totalCount) {
                            options.totalCount = totalCount;
                            options.totalPages = Math.ceil(options.totalCount / options.pageSize);

                        }
                        //移除遮罩
                        // cover.Remove();
                        UI.Hide();

                        pagerBuilder();

                        if (options.totalCount != 0 && options.afterBindDataEvent != null && $.isFunction(options.afterBindDataEvent)) {
                            options.afterBindDataEvent();
                        }
                    }

                    //resize
                    $(window).resize();
                },
                error: function (e) {
                    if (e.responseText) {
                        //Ims.UI.tip_alert('alert-danger', e.responseText);
                    } else {
                        //Ims.UI.tip_alert('alert-danger', options.lang.falseDataNote);
                    }
                }
            });
        } else {
            return;
        }
    }

    //生成分页
    function pagerBuilder() {

        var pageIndex = options.currentPage;
        // start page index
        var startPageIndex = pageIndex - (options.numericPagerItemCount / 2);
        if (startPageIndex + options.numericPagerItemCount > options.totalPages)
            startPageIndex = options.totalPages + 1 - options.numericPagerItemCount;
        if (startPageIndex < 1)
            startPageIndex = 1;

        // end page index
        var endPageIndex = startPageIndex + options.numericPagerItemCount - 1;
        if (endPageIndex > options.totalPages)
            endPageIndex = options.totalPages;

        //分页数据
        var pages = [];
        //上一页
        /*if (pageIndex > 1) {
            var previousPage = pageIndex - 1;
            pages.push({ text: options.lang.previousPage, pageIndex: previousPage });
        }*/
        /*上一页始终显示出来*/
        var previousPage = pageIndex - 1;
        pages.push({ text: options.lang.previousPage, pageIndex: previousPage });

        //more before
        var index;
        if (startPageIndex > 1) {
            index = startPageIndex - 1;
            if (index < 1) index = 1;
            pages.push({ text: "&hellip;", pageIndex: index });
        }
        if (endPageIndex <= 0) {
            endPageIndex = 1;
        }
        //middle
        for (var i = startPageIndex; i <= endPageIndex; i++) {
            pages.push({ text: i, pageIndex: i });
        }
        //more after
        if (endPageIndex < options.totalPages) {
            index = startPageIndex + options.numericPagerItemCount;
            if (index > options.totalPages) {
                index = options.totalPages;
            }
            pages.push({ text: "&hellip;", pageIndex: index });
        }
        //下一页
        /*if (pageIndex < options.totalPages) {
            var nextPageIndex = pageIndex + 1;
            pages.push({ text: options.lang.nextPage, pageIndex: nextPageIndex });
        }*/
        /*下一页始终显示出来*/
        var nextPageIndex = pageIndex + 1;
        pages.push({ text: options.lang.nextPage, pageIndex: nextPageIndex });
        //生成html分页 

        var pageNav = "<div class=\"data-webpage am-u-lg-12 am-cf\"><div class=\"am-fr\"><ul class=\"am-pagination tpl-pagination\">";

        for (var p in pages) {
            if (pages.hasOwnProperty(p)) {
                var $class = "";
                if (pages[p].pageIndex === pageIndex)
                    $class += "class='am-active'";
                if (pages[p].text == "«") {
                    $class += "class='last'";
                    pageNav += "<li " + $class + "><a rel='" + pages[p].pageIndex + "' href='javascript:;'>" + pages[p].text + "</a></li>";
                }
                else if (pages[p].text == "»") {
                    $class += "class='next'";
                    pageNav += "<li " + $class + "><a rel='" + pages[p].pageIndex + "' href='javascript:;'>" + pages[p].text + "</a></li>";
                } else {
                    pageNav += "<li " + $class + "><a rel='" + pages[p].pageIndex + "' href='javascript:;'>" + pages[p].text + "</a></li>";
                }
            }
        }
        pageNav += "</ul></div></div>";


        //绑定之前删除
        selector.parent().find(".data-webpage").remove();
        //绑定分页结果
        selector.after(pageNav);

        /*增加上一页和下一页的样式*/
        if (pageIndex <= 1) {
            selector.next().find("li:first").addClass("am-disabled");
            selector.next().find("li:first").attr("style", "color: #777; cursor: not-allowed;");

            //selector.next().find("li:first").attr("class", "disabled");
        }
        if (pageIndex >= options.totalPages) {
            selector.next().find("li:last").addClass("am-disabled");
            selector.next().find("li:last").attr("style", "color: #777; cursor: not-allowed;");

            //selector.next().find("li:last").attr("class", "disabled");
        }

        bindEvent();
    }

    bindData();
};

/*******************************
   * AJAX等待锁对象遮罩
   * useroptions :  JSON参数对象
   * @obj :         要遮掉的JQUERY DOM对象
   * @opacity:      不透明度 0-1  未公开
   * @bgcolor:      背景颜色  未公开
   * @id :          唯一ID,创建了唯一ID后可以防止重复创建遮罩
   * @zindex:       Z轴深度
   * return :       返回一个对象,对象包含有可调用的Remove方法
   *******************************/
UI.Mask = function (optionsin) {

    var options = $.extend({
        obj: $('body'),
        opacity: 0.2,
        bgcolor: '#dadada',
        id: 'mask_' + Math.round(Math.random() * 10000000),
        zindex: 99
    }, optionsin);
    var obj = options.obj;
    //不传入对象 
    //如果已存在遮罩 直接退出
    if (typeof obj == 'undefined' || $('#' + options.id).length > 0) return;
    var $maskFrame = $(''
        + '<div id="' + options.id + '">'
        + ' <div style="position:absolute;top:0;left:0;height:' + obj.height() + 'px;width:' + obj.width() + 'px;"></div>'
        + ' <div class="mask_screen" style="height:' + obj.height() + 'px;width:' + obj.width() + 'px;position:absolute;top:0;left:0;color:red;"></div>'
        + ' <div class="mask_content" style="height:100%;width:100%;position:absolute;top:0;left:0;">'
        + '     <div style="text-align:center;margin-top:' + obj.height() / 2 + 'px">'
        + '         <div class="loading-spinner fade in" style="width: 200px; margin: 0 auto; z-index: 1050;">'
        + '             <div class="progress progress-striped active">'
        + '                 <div class="progress-bar" style="width: 100%;"></div>'
        + '             </div>'
        + '         </div>'
        + '     </div>'
        + ' </div>'
        + '</div>'
    );

    $maskFrame.css({
        position: 'absolute',
        width: obj.width(),
        height: obj.height(),
        // top: obj.offset().top,
        top: 0,
        left: 0,
        zIndex: options.zindex
    })

        .find('.mask_screen').css({
            opacity: options.opacity,
            backgroundColor: '#dadada'
        })
        .find('.mask_content').css({
            zIndex: options.zindex
        })
        .find('iframe').css({
            opacity: 0
        });

    obj.append($maskFrame);

    var _overflow = obj.css('overflow');
    obj.css({ overflow: 'hidden' });

    return {
        Remove: function () {
            $maskFrame.remove();
            obj.css({ overflow: _overflow });
        }
    };
};

/*
    日期格式化  
    格式 YYYY/yyyy/YY/yy 表示年份  
    MM/M 月份  
    W/w 星期  
    dd/DD/d/D 日期  
    hh/HH/h/H 时间  
    mm/m 分钟  
    ss/SS/s/S 秒  
*/
Date.prototype.format = function (formatStr) {
    if (formatStr === undefined || formatStr === null)
        return;
    var str = formatStr;
    var week = ["日", "一", "二", "三", "四", "五", "六"];

    str = str.replace(/yyyy|YYYY/, this.getFullYear());
    str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : "0" + (this.getYear() % 100));

    str = str.replace(/MM/, this.getMonth() >= 9 ? (this.getMonth() + 1).toString() : "0" + (this.getMonth() + 1));
    str = str.replace(/M/g, this.getMonth() + 1);

    str = str.replace(/w|W/g, week[this.getDay()]);

    str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : "0" + this.getDate());
    str = str.replace(/d|D/g, this.getDate());

    str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : "0" + this.getHours());
    str = str.replace(/h|H/g, this.getHours());
    str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : "0" + this.getMinutes());
    str = str.replace(/m/g, this.getMinutes());

    str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : "0" + this.getSeconds());
    str = str.replace(/s|S/g, this.getSeconds());

    return str;
}