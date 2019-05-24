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
         * 全局设置ajax请求时处理方法前面的根路径
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
            return (value === undefined || value === null) || (!allowEmptyString ? value === '' : false) || ($.isArray(value) && value.length === 0);
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
        /**
         * ajax成功时的处理
         * @param {any} r ajax响应的信息
         * @param {function} success 成功的回调
         * @param {failure} failure 失败的回调
         */
        ajaxSuccess(r, success, failure) {
            const contentType = r.getResponseHeader('content-type'),
                isJson = /json/i.test(contentType);
            let result = isJson && r.responseJson ? r.responseJson : r.responseText,
                succeed = true,// 请求成功
                data; //数据
            if (!hxCore.isEmpty(result) && hxCore.isString(result) && isJson) {
                try {
                    result = $.parseJSON(result);
                } catch (e) {
                    data = result
                }
            }
            data = result;
            if (result) {
                if (result.hasOwnProperty('Success')) {
                    succeed = result.Success;
                    if (result.hasOwnProperty('Resultdata')) {
                        data = result.Resultdata;
                    }
                }
            }
            // result返回结果中Success=true
            if (succeed) {
                // 错误码<=-1 显示警告
                if (hxCore.isNumber(result.Code) && result.Code < 0 && !hxCore.isEmpty(result.Message)) {
                    if (window.alertify) {
                        window.alertify.set('notifier', 'position', 'top-center');
                        window.alertify.error(result.Message).delay(2);
                    } else {
                        _remind({
                            message: result.Message,
                            error: true,
                            timeout: 2000
                        });
                    }
                }
                if (hxCore.isFunction(success)) {
                    success.call(this, data);
                }
            } else {
                const msg = result.Message || '',
                    errcode = result.Code;
                if (hxCore.isFunction(failure)) {
                    failure.call(this, msg, errcode);
                } else if (!hxCore.isEmpty(msg)) {
                    if (window.alertify) {
                        alertify.alert(msg).set({
                            title: '系统提示',
                            transition: 'zoom'
                        });
                    } else {
                        _remind(msg, false);
                    }
                }
            }
        },
        /**
         * ajax请求时失败的处理方法
         * @param {any} r ajax响应的信息
         * @param {any} failure 失败时的回调方法
         */
        ajaxError(r, failure) {
            const contentType = r.getResponseHeader('content-type'),
                isJson = /json/i.test(contentType);
            let error = isJson && r.responseJson ? r.responseJson : r.responseText; //数据
            if (!hxCore.isEmpty(error) && hxCore.isString(error) && isJson) {
                try {
                    error = $.parseJSON(error);
                } catch (e) { error = error; }
            } else {
                error = r.statusText;
            }
            const msg = error.Message || error;
            if (hxCore.isFunction(failure)) {
                failure.call(this, msg, r.status, error);
            } else {
                if (window.alertify) {
                    window.alertify.set('notifier', 'position', 'top-center');
                    window.alertify.error(msg || '服务器忙，请稍后重试!');
                } else {
                    _remind(msg || '服务器忙，请稍后重试!', false);
                }
            }
        },
        /**
         * ajax请求
         * @param {any} url 请求的url
         * @param {any} options 请求的参数
         * @returns {Promise} promise对象
         */
        ajax(url, options) {
            if (arguments.length === 0 || !hxCore.isString(url) || hxCore.isEmpty(url)) {
                hxCore.remindError("ajax 参数不正确");
                return;
            }
            var opt = _handleOptions(options),
                success = opt.success || opt.done,
                error = opt.error || opt.failure,
                complete = opt.complete || opt.always;
            delete opt.success;
            delete opt.done;
            delete opt.error;
            delete opt.failure;
            delete opt.complete;
            delete opt.always;

            if (opt.maskTarget) {
                hxLoad.blockUI(opt.maskTarget);
            }
            if (opt.button) {
                $.each(opt.button, function (key, $b) {
                    $b.attr('disabled', 'disabled');
                });
            }
            var request = $.ajax(hxCore.applyIf({
                url: url,
                type: opt.type || opt.method || 'POST',
                beforeSend: function () {
                    if (hxCore.isFunction(opt.beforeSend)) {
                        opt.beforeSend.call(this, arguments);
                    }
                }
            }, opt));
            request.done(function (data, opts, response) {
                hxCore.ajaxSuccess(response, success, error);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                hxCore.ajaxError(jqXHR, error);
            }).always(function (r, textStatus) {
                if (opt.button) {
                    $.each(opt.button, function (k, $b) {
                        $b.removeAttr('disabled');
                    });
                }
                if (hxCore.isObject(opt.maskTarget)) {
                    hxLoad.unblockUI(opt.maskTarget.target);
                }
                if (hxCore.isFunction(complete)) {
                    complete.call(this, r, textStatus);
                }
            });
            return request;
        },
        /**
         * 成功的提醒
         * @param {any} content 提醒的内容
         */
        remindSuccess(content) {
            if (window.alertify) {
                alertify.set('notifier', 'position', 'top-center');
                alertify.set('notifier', 'delay', 2);
                alertify.success(content);
            } else {
                var opt = {
                    message: content,
                    success: true,
                    valign: 'top',
                    halign: 'center',
                    timeout: 2000
                };
                _remind(opt);
            }
        },
        /**
         * 错误的提醒
         * @param {any} content 提醒的内容
         */
        remindError(content) {
            if (window.alertify) {
                alertify.set('notifier', 'position', 'top-center');
                alertify.set('notifier', 'delay', 2);
                alertify.error(content);
            } else {
                var opt = {
                    message: content,
                    error: true,
                    valign: 'top',
                    halign: 'center',
                    timeout: 2000
                };
                _remind(opt);
            }
        },

    };
    var hxLoad = null;
    if ($.blockUI) {
        hxLoad = {
            blockUI: function (opt) {
                opt = $.extend(true, {}, opt);
                var me = this,
                    loadCls = 'hx-loading-fading-circle',
                    html = '',
                    color = (opt.color || '').toLowerCase(),
                    label = '加载中...',
                    labelAlign = (opt.labelAlign || '').toLowerCase(),
                    hxStyle = '';
                //信息
                if (opt.label === false) {
                    label = '';
                } else if (opt.label) {
                    label = opt.label === true ? label : opt.label;
                }
                //信息方向
                if (labelAlign === 'left' || labelAlign === 'l') {
                    hxStyle += 'flex-direction: row-reverse;-webkit-flex-direction: row-reverse;';
                } else if (labelAlign === 'top' || labelAlign === 't') {
                    hxStyle += 'flex-direction: column-reverse;-webkit-flex-direction: column-reverse;';
                } else if (labelAlign === 'bottom' || labelAlign === 'b') {
                    hxStyle += 'flex-direction: column;-webkit-flex-direction: column;';
                } else {
                    hxStyle += 'flex-direction: row;-webkit-flex-direction: row;';
                }
                //信息模式
                var mode = (opt.mode || '').toLowerCase();
                if (mode === 'bounce' || mode === 'b') {
                    loadCls = 'hx-loading-bounce';
                    html = `<div class='hx-loading' style="${hxStyle}">
                        <div class="${loadCls} hx-loading-${color}">
                            <div class="bounce-child bounce1"></div>
                            <div class="bounce-child bounce2"></div>
                            <div class="bounce-child bounce3"></div>
                        </div>
                        <span>&nbsp;&nbsp;${label}
                    </div>
                `;
                } else if (mode === 'circle' || mode === 'c') {
                    loadCls = 'hx-loading-circle';
                    html += `<div class='hx-loading' style="${hxStyle}">`;
                    html += `<div class="${loadCls} hx-loading-${color}">`;
                    for (var i = 1; i <= 12; i++) {
                        html += `<div class="circle${i} circle-child"></div>`;
                    }
                    html += '</div>';
                    html += `<span style='display:block;'>&nbsp;&nbsp;${label}`;
                    html += '</div>';
                } else {
                    html += `<div class='hx-loading' style="${hxStyle}">`;
                    html += `<div class="${loadCls} hx-loading-${color}">`;
                    for (var j = 1; j <= 12; j++) {
                        html += `<div class="fadecircle${j} circle-child"></div>`;
                    }
                    html += '</div>';
                    html += `<span>&nbsp;&nbsp;${label}`;
                    html += '</div>';
                }
                var blockOpt = {
                    message: html,
                    baseZ: opt.zIndex ? opt.zIndex : 1000,
                    centerY: opt.cenrerY !== undefined ? opt.cenrerY : true,
                    centerX: opt.centerX !== undefined ? opt.centerX : true,
                    // (hat tip to Jorge H. N. de Vasconcelos) 
                    iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank',
                    // force usage of iframe in non-IE browsers (handy for blocking applets) 
                    forceIframe: false,
                    // fadeIn time in millis; set to 0 to disable fadeIn on block 
                    fadeIn: opt.fadeIn ? opt.fadeIn : 200,
                    // fadeOut time in millis; set to 0 to disable fadeOut on unblock 
                    fadeOut: opt.fadeOut ? opt.fadeOut : 400,
                    // time in millis to wait before auto-unblocking; set to 0 to disable auto-unblock 
                    timeout: opt.timeout ? opt.timeout : 0,
                    css: {
                        border: '0',
                        padding: '0',
                        backgroundColor: 'none'
                    },
                    overlayCSS: {
                        backgroundColor: opt.overlayColor ? opt.overlayColor : '#555',
                        opacity: opt.opacity ? opt.opacity : 0.1,
                        cursor: 'wait'
                    }
                };
                if (opt.target) {
                    var el = $(opt.target);
                    if (el.height() <= $(window).height()) {
                        blockOpt.cenrerY = true;
                    }
                    el.block(blockOpt);
                } else { // page blocking
                    $.blockUI(blockOpt);
                }
            },
            unblockUI: function (el) {
                if (el) {
                    $(el).unblock({
                        onUnblock: function () {
                            $(el).css('position', '');
                            $(el).css('zoom', '');
                        }
                    });
                } else {
                    $.unblockUI();
                }
            }
        };
    }
    var _handleOptions = function (options) {
        options = options || {};
        // 遮罩层
        if (hxLoad && options.maskTarget) {
            var maskOpt = {};
            if (options.maskTarget === true) {
                maskOpt = {};
            } else if (hxCore.isString(options.maskTarget)) {
                if ($(options.maskTarget).length > 0) {
                    maskOpt.target = $(options.maskTarget);
                } else {
                    maskOpt.label = options.maskTarget;
                }
            } else if (hxCore.isObject(options.maskTarget)) {
                if (options.maskTarget instanceof jQuery) {
                    maskOpt.target = options.maskTarget;
                } else {
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
        } else if (opt.warning) {
            $dialog.addClass('alert alert-warning alert-dismissable');
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
        if (timeout > 0) {
            setTimeout(function () {
                $dialog.remove();
                if (hxCore.isFunction(opt.callback)) {
                    opt.callback.call(this, $dialog);
                }
            }, timeout);
        }
    }
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
    
    
    window.HxCore = $.extend(true, hxCore, hxLoad, {
        /**
         * 自定义的提醒
         * @param {any} opt 配置
         */
        remind(opt) {
            if (window.alertify) {
                var type = 'message';
                if (opt.success) {
                    type = 'success';
                }
                if (opt.error) {
                    type = 'error';
                } else if (opt.warning) {
                    type = 'warning';
                }
                var valign = opt.valign || 'top',
                    halign = opt.halign || 'center';
                alertify.set('notifier', 'position', valign + '-' + halign);
                alertify.notify(opt.message, type, opt.timeout / 1000, opt.callback);
            } else {
                _remind(opt);
            }
        },
        formatString() {
            _formatString(arguments);
        }
    });
})(jQuery, window);