(function ($, window) {
    $.extend({
        queryString: function (name) {
            var search = decodeURIComponent(window.location.search || '');
            if (search.charAt(0) != '?') {
                return undefined;
            } else {
                search = search.replace('?', '').split('&');
                for (var i = 0; i < search.length; i++) {
                    if ((search[i].split('=')[0] || '').trim() == name) {
                        return decodeURI(search[i].split('=')[1]);
                    }
                }
                return undefined;
            }
        },
        isEmpty: function (value, allowEmptyString) {
            return (value == null) || (!allowEmptyString ? value === '' : false) || ($.isArray(value) && value.length === 0);
        },
        isString: function (value) {
            return typeof value === 'string';
        },
        /**
         * 如果传递的值是JavaScript对象，则返回“true”，否则返回“false”。
         * @param {Object} value 要测试的值。
         * @return {Boolean}
         * @method
         */
        isObject: (toString.call(null) === '[object Object]') ?
            function (value) {
                // 在这里检查ownerDocument以排除DOM节点
                return value != null && toString.call(value) === '[object Object]' && value.ownerDocument === undefined;
            } :
            function (value) {
                return toString.call(value) === '[object Object]';
            },

        /**
         * @private
         */
        isSimpleObject: function (value) {
            return value instanceof Object && value.constructor === Object;
        },
    });

})(jQuery, window)