
var myTeamPager = myTeamPager || {};

//数据处理类
myTeamPager.control = {
    pageIndex: 1,
    pageSize: 10,
    teamLevel: 0,
    topHeight: 332,
    GradeEnum: null,
    imageDomain: '',
    getData: function () {
        var me = this;
        $.ajax({
            type: "post",
            url: me.url,
            dataType: "json",
            data: { Level: me.teamLevel, CurrentPageIndex: me.pageIndex, PageSize: me.pageSize },
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
                    html += "<li><div class=\"con5l f\"><img src=\"" + me.imageDomain + item.HeadImgUrl + "\" style=\"height:94px; width:94px;\"></div>";

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

                    html += "<div class=\"con5m f xi18\"><span class=\"xi24\">" + item.RealName + " " + item.PhoneNumber + "</span><br><span class=\"hui\">" + customerGradeName + "（有效期 " + item.ExpiresTime + "）<br>注册时间 " + $.vailCenter.conertJsonTimeAndFormat(item.CreatedDate, "yyyy-MM-dd HH:mm") + "</span></div>";
                    html += "<a href=\"tel:" + item.PhoneNumber + "\"><div class=\"con5r r xi22 cen\">打电话</div></a></li>";
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
            top: myTeamPager.control.topHeight,
            bottom: 0,
            endHtml: '<div class=\"con6 mgz cen xi22\">没有更多数据</div>',
            extraId: "divPager",
            noDataHtml: '<div class=\"con6 mgz cen xi22\">没有更多数据</div>'
        });
    }
};
//事件注册对象
myTeamPager.handler = {
    init: function () {
        var level = $("#levelHide").val();
        $("#divContent").html("");
        myTeamPager.control.teamLevel = level;

        if (level == -1) {
            $("#divTeam").show();
            $("#wrapper").hide();
            $("#divPager").hide();
        } else {
            $("#divTeam").hide();
            $("#wrapper").show();
            $("#divPager").show();
            myTeamPager.control.getData();
        }
    }
};