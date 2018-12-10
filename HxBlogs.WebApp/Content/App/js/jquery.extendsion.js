(function ($, windoe) {
    $.extend({
        queryString: function (name) {
            var search = window.location.search || '';
            if (search.charAt(0) != '?') {
                return undefined;
            } else {
                search = search.replace('?', '').split('&');
                for (var i = 0; i < search.length; i++) {
                    if (search[i].split('=')[0] == name) {
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
    });

})(jQuery, window)