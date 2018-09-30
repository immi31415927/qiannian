var cashoutLogPager = cashoutLogPager || {};

//数据处理类
cashoutLogPager.control = {
    pageIndex: 1,
    pageSize: 10,
    teamLevel: 0,
    topHeight: 175,
    statusEnum: null,
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

                    var statusName = "";


                    if (me.statusEnum.驳回 == item.Status) {
                        statusName = "驳回";
                    }
                    if (me.statusEnum.处理中 == item.Status) {
                        statusName = "处理中";
                    }
                    if (me.statusEnum.完成 == item.Status) {
                        statusName = "完成";
                    }


                    html += "<li>";
                    html += "<div class=\"con5ls f xi22\">";
                    html += "提现时间&nbsp;&nbsp;<span class=\"xi18 hui\">" + $.vailCenter.conertJsonTimeAndFormat(item.CreatedDate, "yyyy-MM-dd HH:mm") + "</span><br>";
                    html += "提现金额&nbsp;&nbsp;<span class=\"xi18 hui\">" + item.Amount + "</span>";
                    html += "</div>";
                    html += "<a href=\"#\"><div class=\"con5r r xi22 cen\" style=\"margin-top:14px\">" + statusName + "</div></a>";
                    html += "</li>";

                    if (me.statusEnum.驳回 == item.Status) {
                        html += "<li>";
                        html += "<div class=\"con5ls f xi22\">";
                        html += "驳回原因&nbsp;&nbsp;<span class=\"xi18 hui\">" + item.Reject + "</span><br>";
                        html += "</div>";
                        html += "</li>";
                    }

                }


                //Reject
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
            top: cashoutLogPager.control.topHeight,
            bottom: 0,
            endHtml: '<div class=\"con6 mgz cen xi22\">没有更多数据</div>',
            extraId: "divPager",
            noDataHtml: '<div class=\"con6 mgz cen xi22\">没有更多数据</div>'
        });
    }
};
//事件注册对象
cashoutLogPager.handler = {
    init: function () {
        var level = $("#levelHide").val();
        $("#divContent").html("");
        cashoutLogPager.control.teamLevel = level;

        if (level == -1) {
            $("#divTeam").show();
            $("#wrapper").hide();
            $("#divPager").hide();
        } else {
            $("#divTeam").hide();
            $("#wrapper").show();
            $("#divPager").show();
            cashoutLogPager.control.getData();
        }
    }
};

