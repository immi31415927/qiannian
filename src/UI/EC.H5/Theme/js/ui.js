var UI = {
    Tips: function (content) {
        layer.open({
            content: content,
            skin: 'msg',
            time: 2 //
        });
    },
    Alert: function (options) {
        var opts = $.extend({
            title: '系统消息',
            content: '',
            btnValue: '确定',
            callback: function () {
            }
        }, options);
        layer.open({
            content: opts.content,
            shadeClose: false,
            btn: opts.btnValue,
            yes: function (index) {
                opts.callback();
                layer.close(index);
            }
        });
    },
    Confirm: function (options) {
        var opts = $.extend({
            content: '',
            okBtnValue: '确定',
            okCallBack: function () {
            },
            cancelBtnValue: '取消',
            cancelCallBack: function () {
            }
        }, options);

        layer.open({
            content: opts.content,
            shadeClose: false,
            btn: [opts.okBtnValue, opts.cancelBtnValue],
            yes: function (index) {
                opts.okCallBack();
                layer.close(index);
            },
            no: function (index) {
                opts.cancelCallBack();
                layer.close(index);
            }
        });
    },
    /*******************************
    loading
    {
        @ content :     内容,即原当前对象
        @ isClear :     是否自动消失,即清除原有所有内容
        @ callBack :    回调函数
    }
    *******************************/
    Loading: function () {
        var isIe = (document.all) ? true : false;
        var isIe6 = isIe && !window.XMLHttpRequest;
        var position = !isIe6 ? "fixed" : "absolute";

        var maxHeight = Math.max(document.documentElement.scrollHeight, document.documentElement.clientHeight) + "px";
        var marginTop = document.documentElement.clientHeight / 2 + "px";


        var template = '<div class="loadingBox" style="background: rgba(255,255,255,.4);position: fixed;width: 100%;height: ' + maxHeight + ';z-index:9999;top:0%;"><div class="spinner" style="width: 100%;text-align: center;z-index: 999;margin-top: ' + marginTop + ';"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div></div>';
        if ($(".loadingBox").hasClass) {
            $(".loadingBox").remove();
        }
        $("body").append(template);
    },
    Hide: function () {
        if ($(".loadingBox").hasClass) {
            $(".loadingBox").remove();
        }
    },
    Ajax: function (opt) {
        var fn = {
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
            success: function (data, textStatus) { }
        }
        if (opt.error) {
            fn.error = opt.error;
        }
        if (opt.success) {
            fn.success = opt.success;
        }
        var _opt = $.extend(opt, {
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                fn.error(XMLHttpRequest, textStatus, errorThrown);
            },
            success: function (data, textStatus) {
                try {
                    if (data.length < 500) {
                        var dataJson = $.parseJSON(data);
                        if (dataJson.Error) {
                            UI.Tips(dataJson.Message);
                            data = "";
                        }
                    }
                    if (typeof (data) === "object") {
                        if (data.IsLogout) {
                            UI.Tips(dataJson.Message);
                            data = "";
                        }
                    }
                } catch (e) {
                }
                fn.success(data, textStatus);
            }
        });
        $.ajax(_opt);
    }
};