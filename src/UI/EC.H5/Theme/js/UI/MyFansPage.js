
var myFansPager = myFansPager || {};

//数据处理类
myFansPager.control = {
    pageIndex: 1,
    pageSize: 10,
    GradeEnum: null,
    sendInfoUrl: undefined,
    getData: function () {
        var me = this;
        $.ajax({
            type: "post",
            url: me.url,
            dataType: "json",
            data: { CurrentPageIndex: me.pageIndex, PageSize: me.pageSize },
            success: function (json) {
                if (!json.Status) {
                    UI.Alert({ content: json.Message });
                    return;
                }

                me.totalSize = parseInt((json.Count - 1) / me.pageSize + 1);
                me.tables = json.Data;
                me.setTable();
            },
            error: function (json) {
                UI.Alert({ content: "网络故障,请查看网络" });
            }
        });
    },
    setTable: function () {
        var me = this;
        $.mPage.bPage({
            textObjId: "divContent",
            callback: function () {
                var html = "";
                var tables = me.tables;
                for (var i = 0; i < tables.length; i++) {
                    var item = tables[i];
                    html += "<li><div class=\"con5l f\"><img src=\"" + item.HeadImgUrl + "\" style=\"height:94px; width:94px;\"></div><div class=\"con5m f xi18\"><span class=\"xi24\">";

                    if (item.IsRegister == 0) {
                        html += item.Nickname + "</span><br><span class=\"hui\">未注册<br>关注时间 " + $.vailCenter.conertJsonTimeAndFormat(item.CreatedDate, "yyyy-MM-dd HH:mm") + "</span></div><a href=\"" + me.sendInfoUrl + "?sysNo=" + item.SysNo + "\"><div class=\"con5r r xi22 cen\">发消息</div></a></li>";
                    }
                    else {
                        var customerGradeName = "注册会员";
                        if (me.GradeEnum.普通会员 == item.Grade) {
                            customerGradeName = "普通会员";
                        }
                        if (me.GradeEnum.普通代理 == item.Grade) {
                            customerGradeName = "普通代理";
                        }
                        if (me.GradeEnum.区域代理 == item.Grade) {
                            customerGradeName = "区域代理";
                        }
                        if (me.GradeEnum.全国代理 == item.Grade) {
                            customerGradeName = "全国代理";
                        }
                        if (me.GradeEnum.股东 == item.Grade) {
                            customerGradeName = "股东";
                        }

                        html += item.Nickname + " " + item.TelNumber + "</span><br><span class=\"hui\">" + customerGradeName + "<br>关注时间 " + $.vailCenter.conertJsonTimeAndFormat(item.CreatedDate, "yyyy-MM-dd HH:mm") + "</span></div><a href=\"tel:" + item.TelNumber + "\"><div class=\"con5r r xi22 cen\">打电话</div></a>";
                    }
                }

                return html;
            },
            getData: function (direction) {
                if (direction == "down") {
                    me.pageIndex = 1;
                } else {
                    me.pageIndex++;
                }

                me.getData();
            },
            pgObjs: me.pageIndex,
            sizes: me.totalSize,
            top: 180,
            bottom: 0,
            endHtml: '<div class=\"con6 mgz cen xi22\">没有更多数据</div>',
            extraId: "divPager",
            noDataHtml: '<div class=\"con6 mgz cen xi22\">没有更多数据</div>'
        });
    }
};
//事件注册对象
myFansPager.handler = {
    init: function () {
        //$("html").addClass("container-fluid");
        myFansPager.control.getData();
        this.ulClick();
    },
    ulClick: function () {

    }
};