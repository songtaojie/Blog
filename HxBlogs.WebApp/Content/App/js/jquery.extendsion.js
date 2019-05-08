(function ($, window) {
    var _handleOptions = function (options) {
        const me = this;
        options = options || {};
        // 遮罩层
        if (options.maskTarget) {
            if (options.maskTarget === true) {
                options.maskTarget = '加载中';
            } else if ($.isString(options.maskTarget)) {
                options.maskTarget = options.maskTarget;
            }
        }
        // 此ajax请求时需要禁用的按钮，可为空
        if (options.button && !$.isEmpty(options.button)) { // 也就是防止用户连续点击按钮
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
        options.params = Ext.apply({}, options.params);
        return options;
    };
    $.extend({
        queryString: function (name) {
            var search = decodeURIComponent(window.location.search || '');
            if (search.charAt(0) !== '?') {
                return undefined;
            } else {
                search = search.replace('?', '').split('&');
                for (var i = 0; i < search.length; i++) {
                    if ((search[i].split('=')[0] || '').trim() === name) {
                        return decodeURI(search[i].split('=')[1]);
                    }
                }
                return undefined;
            }
        },
        isEmpty: function (value, allowEmptyString) {
            return (value === null) || (!allowEmptyString ? value === '' : false) || ($.isArray(value) && value.length === 0);
        },
        isString: function (value) {
            return typeof value === 'string';
        },
        /**
         * 如果传递的值是JavaScript对象，则返回“true”，否则返回“false”。
         * @param {Object} value 要测试的值。
         * @returns {Boolean} 返回的值
         */
        isObject: (toString.call(null) === '[object Object]') ?
            function (value) {
                // 在这里检查ownerDocument以排除DOM节点
                return value !== null && toString.call(value) === '[object Object]' && value.ownerDocument === undefined;
            } :
            function (value) {
                return toString.call(value) === '[object Object]';
            },

        /**
         *  @param {Object} value 要测试的值。
         *  @returns {Boolean} 返回的值
         */
        isSimpleObject: function (value) {
            return value instanceof Object && value.constructor === Object;
        },
    });

})(jQuery, window);