if (!jQuery) { throw new Error("hxCore requires jQuery"); }
; (function ($, window) {
    "use strict";
    /**
     * 生成四个随机十六进制数字
     * @returns {String} 四个随机十六进制数字
     */
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }; 
    var intRex = /^[-+]?\d+(?:[Ee]\+?\d+)?$/,
        floatRex = /^[-+]?(?:\d+|\d*\.\d*)(?:[Ee][+-]?\d+)?$/,
        _ajaxRootUrl = '';
    var hxCore = {
        /**
         * 设置ajax请求时处理方法前面的根路径
         * @param {String} root 根路径
         */
        setAjaxRootUrl(root) {
            if (hxCore.isString(root)) {
                _ajaxRootUrl = root;
            }
        },
        /**
         * 判断给定值是否为空
         * @param {any} value 要判断的值
         * @param {Boolean} allowEmptyString 是否允许空字符串
         * @returns {Boolean} true为空，false不为空
         */
        isEmpty(value, allowEmptyString) {
            return (value === null) || (!allowEmptyString ? value === '' : false) || ($.isArray(value) && value.length === 0);
        },
        /**
         * 判断给定值是否是字符串
         * @param {any} value 要验证的值
         * @returns {Boolean} true代表是字符串，false代表不是字符串
         */
        isString(value) {
            return typeof value === 'string';
        },
        /**
         * 判断给定制是否是对象
         * @param {any} value 要验证的值
         * @returns {Boolean} true代表是对象，false代表不是对象
         */
        isObject(value) {
            if (toString.call(null) === '[object Object]') {
                //在这里检查ownerDocument以排除DOM节点
                return value !== null && toString.call(value) === '[object Object]' && value.ownerDocument === undefined;
            } else {
                return toString.call(value) === '[object Object]';
            }
        },
        /**
         * 是否是简单地对象
         * @param {any} value 要验证的值
         * @returns {Boolean} true代表是简单对象，false代表不是简单对象
         */
        isSimpleObject(value) {
            return value instanceof Object && value.constructor === Object;
        },
        /**
         * 是否是boolean
         * @param {any} value 要验证的值
         * @returns {Boolean} true代表是Bool值，false代表不是Bool值
         */
        isBoolean(value) {
            return typeof value === 'boolean';
        },
        /**
         * 如果传递的值是数字，则返回“true”。 对于非有限数，返回`false`。
         * @param {Object} value 要测试的值。
         * @return {Boolean} 如果传递的值是数字，则返回“true”。 对于非有限数，返回`false`。
         */
        isNumber(value) {
            return typeof value === 'number' && isFinite(value);
        },
        /**
         * 如果传递的值是JavaScript函数则返回“true”，否则返回“false”。
         * @param {Object} value 要测试的值.
         * @return {Boolean} 如果传递的值是JavaScript函数则返回“true”，否则返回“false”。
         */
        isFunction:
            (typeof document !== 'undefined' && typeof document.getElementsByTagName('body') === 'function') ? function (value) {
                return !!value && toString.call(value) === '[object Function]';
            } : function (value) {
                return !!value && typeof value === 'function';
            },
        /**
        *严格解析给定值并将值作为数字返回，如果值不是整数或包含非整数块，则返回“null”。
        * @param {String} value 要测试的值。
        * @return {Number} 严格解析给定值并将值作为数字返回，如果值不是整数或包含非整数块，则返回“null”。
        */
        parseInt(value) {
            if (value === undefined) {
                value = null;
            }

            if (typeof value === 'number') {
                value = Math.floor(value);
            }
            else if (value !== null) {
                value = String(value);
                value = intRex.test(value) ? +value : null;
            }

            return value;
        },
        /**
         * 严格解析给定值并将值作为数字返回，如果值不是数字或包含非数字片段，则返回“null”。
         * @param {String} value 要测试的值。
         * @return {Number} 严格解析给定值并将值作为数字返回，如果值不是数字或包含非数字片段，则返回“null”。
         */
        parseFloat(value) {
            if (value === undefined) {
                value = null;
            }

            if (value !== null && typeof value !== 'number') {
                value = String(value);
                value = floatRex.test(value) ? +value : null;
                if (isNaN(value)) {
                    value = null;
                }
            }

            return value;
        },
        guid() {
            return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
        },
        /**
         * 将config的所有属性复制到对象（如果它们尚不存在）。
         * @param {Object} object 属性的接收者
         * @param {Object} config 属性的来源
         * @return {Object} 操作后的对象
         */
        applyIf(object, config) {
            if (object && config && typeof config === 'object') {
                for (var property in config) {
                    if (object[property] === undefined) {
                        object[property] = config[property];
                    }
                }
            }
            return object;
        },
        ajax(url, options) {
            if (arguments.length === 0 || !hxCore.isString(url) || !hxCore.isEmpty(url)) {
                _remind('ajax 参数不正确', false);
                return;
            }
            var opt = _handleOptions(options),
                success = opt.success || opt.done,
                eoor = opt.error || opt.failure,
                complete = opt.complete || opt.always;
            delete opt.success;
            delete opt.done;
            delete opt.error;
            delete opt.failure;
            delete opt.complete;
            delete opt.always;

            if (opt.maskTarget) {
                HxLoad.blockUI(opt.maskTarget);
            }
            if (opt.button) {
                $.each(opt.button, function (key, value) {
                    value.attr('disabled', 'disabled');
                });
            }
            $.ajax(hxCore.applyIf({
                url: opt.url,
                type: 'POST',
                beforeSend: function () {
                    if (hxCore.isFunction(opt.beforeSend)) {
                        opt.beforeSend.call(this, arguments);
                    }
                }
            }, opt)).done(function (response, opts) {
                const contentType = response.getResponseHeader('content-type'),
                    isJson = /json/i.test(contentType);
                let result = response.responseText,
                    succeed = true; // 请求成功
                });
        }
    };

    var _handleOptions = function (options) {
        options = options || {};
        // 遮罩层
        if (HxLoad) {
            var maskOpt = {};
            if (options.maskTarget) {
                if (options.maskTarget === true) {
                    maskOpt = {};
                } else if (hxCore.isString(options.maskTarget)) {
                    if ($(options.maskTarget).length > 0) {
                        maskOpt.target = $(options.maskTarget);
                    } else {
                        maskOpt.label = options.maskTarget;
                    }
                } else if (hxCore.isObject(options.maskTarget)) {
                    maskOpt = options.maskTarget;
                }
            }
            options.maskTarget = maskOpt;
        } else {
            delete options.maskTarget;
        }
        
        // 此ajax请求时需要禁用的按钮， 也就是防止用户连续点击按钮
        if (options.button && !hxCore.isEmpty(options.button)) {
            const btns = [];
            let bs = options.button;
            if ($.isString(bs)) {
                bs = bs.split(',');
            } else if ($.isArray(bs) || $.isObject(bs)) {
                bs = bs;
            }
            $.each(bs, function (key, value) {
                if ($.isEmpty(value)) return;
                if ($.isString(value) && $(value).length > 0) {
                    btns.push($(value));
                } else if (value instanceof jQuery && value.length > 0) {
                    btns.push(value);
                }
            });
            if (btns.length) {
                options.button = btns;
            } else {
                delete options.button;
            }
        }
        options.params = $.extend({}, options.params);
        return options;
    };

    /**
     * 提醒
     */
    function _remind() {
        var opt = {};
        if (arguments.length === 1) {
            if (hxCore.isString(arguments[0])) {
                opt.message = arguments[0];
            } else if (hxCore.isObject(arguments[0])) {
                $.extend(true, opt, arguments[0]);
            }
        } else if (arguments.length === 2) {
            if (hxCore.isString(arguments[0]) && hxCore.isBoolean(arguments[1])) {
                opt.message = arguments[0];
                opt.success = arguments[1];
                opt.error = !arguments[1];
            }
        }
        var ulStyle = 'text-align: center;position: fixed;padding-top: 10px; padding-bottom: 10px; z-index: 10000000;box-shadow: rgba(0, 0, 0, 0.4) 0 1px 6px;left:0;top:0;';
        //宽度
        var intWidth = null;
        if (opt.width) {
            if (hxCore.isString(opt.width)) {
                var index = opt.width.indexOf('px');
                if (index >= 0) {
                    intWidth = hxCore.parseInt(opt.width.substring(0, index));
                } else {
                    intWidth = hxCore.parseInt(opt.width);
                }
            } else if (hxCore.isNumber(opt.width)) {
                intWidth = opt.width;
            }
        }
        if (hxCore.isEmpty(intWidth) && intWidth <= 0) {
            intWidth = 300;
        }
        ulStyle = `${ulStyle} width:${intWidth}px;`;
        var $dialog = $(document.createElement("div"));
       
        if (opt.success) {
            $dialog.addClass('alert alert-success alert-dismissable');
        } else if (opt.error) {
            $dialog.addClass('alert alert-danger alert-dismissable');
        } else {
            $dialog.addClass('alert alert-info alert-dismissable');
        }
        var $dialogHtmlContent = '<button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times;</button>{0}';
        var $dialogHtml = _formatString($dialogHtmlContent, opt.message);
        $dialog.html($dialogHtml);
        $("body").append($dialog);

        //位置
        var availWidth = document.documentElement && document.documentElement.clientWidth || document.body.clientWidth,
            availHeight = document.documentElement && document.documentElement.clientHeight || document.body.clientHeight;
        var valign = opt.valign || 'top',
                left = availWidth / 2 - intWidth / 2,
                top = 40;
        if (hxCore.isString(valign)) {
            if (valign.toLowerCase() === 'top') {
                top = 40;
            } else if (valign.toLowerCase() === 'bottom') {
                top = availHeight - 180;
            } else if (valign.toLowerCase() === 'center') {
                top = availHeight / 2 - $dialog.height() / 2 - 120;
            }
        }
        var halign = opt.halign || 'center';
        if (hxCore.isString(halign)) {
            if (halign.toLowerCase() === 'left') {
                left = 20;
            } else if (halign.toLowerCase() === 'right') {
                left = availWidth - 20;
            } else if (halign.toLowerCase() === 'center') {
                availWidth / 2 - intWidth / 2;
            }
        }
        ulStyle = `${ulStyle}transform: translate3d(${left}px,${top}px, 0);
            -ms-transform: translate3d(${left}px,${top}px, 0);
            -moz-transform:translate3d(${left}px,${top}px, 0);
            -o-transform: translate3d(${left}px,${top}px, 0);`;
        $dialog.attr("style", ulStyle);
        $dialog.show();
        var timeout = 1500;
        if (opt.timeout && hxCore.isNumber(opt.timeout)) {
            timeout = opt.timeout;
        }
        setTimeout(function () {
            $dialog.remove();
        }, timeout);
    };
    function _formatString() {
        if (arguments.length < 1) {
            return null;
        }
        var str = arguments[0];
        for (var i = 1; i < arguments.length; i++) {
            var placeHolder = '{' + (i - 1) + '}';
            str = str.replace(placeHolder, arguments[i]);
        }

        return str;
    }
    window.HxCore = $.extend(true, hxCore, {
        /**
         * 成功的提醒
         * @param {any} content 提醒的内容
         */
        remindSuccess(content) {
            _remind(content, true);
        },
        /**
         * 错误的提醒
         * @param {any} content 提醒的内容
         */
        remindError(content) {
            _remind(content, false);
        },
        /**
         * 自定义的提醒
         * @param {any} opt 配置
         */
        remind(opt) {
            _remind(opt); 
        },
        formatString() {
            _formatString(arguments);
        }

    });
})(jQuery, window);