//*************************扩展方法****************************
(function ($) {

    $.vailCenter = {
        //验证正整数
        isPositiveInt: function (value) {
            var validateReg = /^[1-9]\d*$/;
            return validateReg.test(value.trim());
        },
        //验证座机号码
        isTel: function (value) {
            var validateReg = /^0\d{2,3}-?\d{7,8}$/;
            return validateReg.test(value.trim());
        },
        //验证手机号码
        isMobile: function (value) {
            var validateReg = /^1[2|3|4|5|6|7|8|9]\d{9}$/;
            return validateReg.test(value.trim());
        },
        //验证邮箱格式
        isEmail: function (value) {
            var validateReg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
            return validateReg.test(value);
        },
        //验证是否为空
        isNullOrEmptySpance: function (value) {
            if (Object.prototype.toString.call(value) === "[object String]") {
                if ((value == null || value.trim() == "" || value == 'undefined' || value == 'null'))
                    return true;
            } else {
                if ((value == null || value == "" || value == 'undefined' || value == 'null'))
                    return true;
            }
            return false;
        },
        //验证为正整数
        isPositiveInteger: function (value) {
            var validateReg = /^[0-9]*$/;
            return validateReg.test(value.trim());
        },
        //身份证验证
        isIdCardNo: function (num) {
            num = num.toUpperCase();
            var factorArr = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1);
            var parityBit = new Array("1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2");
            var varArray = new Array();
            var intValue;
            var lngProduct = 0;
            var intCheckDigit;
            var intStrLen = num.length;
            var idNumber = num;
            // initialize
            if ((intStrLen != 15) && (intStrLen != 18)) {
                return false;
            }
            // check and set value
            for (var i = 0; i < intStrLen; i++) {
                varArray[i] = idNumber.charAt(i);
                if ((varArray[i] < '0' || varArray[i] > '9') && (i != 17)) {
                    return false;
                } else if (i < 17) {
                    varArray[i] = varArray[i] * factorArr[i];
                }
            }
            if (intStrLen == 18) {
                //check date
                var date8 = idNumber.substring(6, 14);
                if (isDate8(date8) == false) {
                    return false;
                }
                // calculate the sum of the products
                for (i = 0; i < 17; i++) {
                    lngProduct = lngProduct + varArray[i];
                }
                // calculate the check digit
                intCheckDigit = parityBit[lngProduct % 11];
                // check last digit
                if (varArray[17] != intCheckDigit) {
                    return false;
                }
            } else { //length is 15
                //check date
                var date6 = idNumber.substring(6, 12);
                if (isDate6(date6) == false) {
                    return false;
                }
            }
            return true;
        },
        conertJsonTimeAndFormat: function (jsonTime, format) {
            //通过正则拿到里面数字。
            var dateValue = jsonTime.replace(/\/Date\((\d+)\)\//gi, '$1'); //g 全局 i不区分大小写
            var date = new Date();
            date.setTime(dateValue); //value通过截取字符串只取数字。

            return formatDate(date, format);
        },
        unixToDateFormat: function (unix, format) {
            var date = new Date();
            date.setTime(unix * 1000);
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            m = m < 10 ? ('0' + m) : m;
            var d = date.getDate();
            d = d < 10 ? ('0' + d) : d;
            var h = date.getHours();
            h = h < 10 ? ('0' + h) : h;
            var minute = date.getMinutes();
            var second = date.getSeconds();
            minute = minute < 10 ? ('0' + minute) : minute;
            second = second < 10 ? ('0' + second) : second;
            var jsonTime = y + '-' + m + '-' + d + ' ' + h + ':' + minute + ':' + second;

            return formatDate(jsonTime, format);
        }
    };

    function isDate6(sDate) {
        if (!/^[0-9]{6}$/.test(sDate)) {
            return false;
        }
        var year, month, day;
        year = "19" + sDate.substring(0, 2);
        month = sDate.substring(2, 4);
        day = sDate.substring(4, 6);
        var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        if (year < 1700 || year > 2500) return false;
        if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
        if (month < 1 || month > 12) return false;
        if (day < 1 || day > iaMonthDays[month - 1]) return false;

        //计算年龄
        var agen = new Date() - new Date(year, month, day);
        var age = Math.round(agen / (365 * 24 * 60 * 60 * 1000));
        if (age > 65 || age < 18) return false;
        return true;
    };

    function isDate8(sDate) {
        if (!/^[0-9]{8}$/.test(sDate)) {
            return false;
        }
        var year, month, day;
        year = sDate.substring(0, 4);
        month = sDate.substring(4, 6);
        day = sDate.substring(6, 8);
        var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        if (year < 1700 || year > 2500) return false;
        if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
        if (month < 1 || month > 12) return false;
        if (day < 1 || day > iaMonthDays[month - 1]) return false;
        //计算年龄
        var nowDate = new Date();
        var date = formatDate(nowDate, 'yyyyMMdd');
        var agenum = (parseInt(date) - parseInt(sDate)).toString();
        var age = agenum.substring(0, agenum.length - 4);
        if (age >= 65 || age < 18) return false;

        return true;
    };

    function formatDate(date, format) {
        if (!date) return;
        if (!format) format = "yyyy-MM-dd";
        switch (typeof date) {
            case "string":
                date = new Date(date.replace(/-/, "/"));
                break;
            case "number":
                date = new Date(date);
                break;
        }
        if (!date instanceof Date) return;
        var dict = {
            "yyyy": date.getFullYear(),
            "M": date.getMonth() + 1,
            "d": date.getDate(),
            "H": date.getHours(),
            "m": date.getMinutes(),
            "s": date.getSeconds(),
            "MM": ("" + (date.getMonth() + 101)).substr(1),
            "dd": ("" + (date.getDate() + 100)).substr(1),
            "HH": ("" + (date.getHours() + 100)).substr(1),
            "mm": ("" + (date.getMinutes() + 100)).substr(1),
            "ss": ("" + (date.getSeconds() + 100)).substr(1)
        };
        return format.replace(/(yyyy|MM?|dd?|HH?|ss?|mm?)/g, function () {
            return dict[arguments[0]];
        });
    };
})(jQuery);