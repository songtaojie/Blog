; if (window.alertify) {
    alertify.defaults.transition = "zoom";

    alertify.defaults.glossary.title = '系统提示';
    alertify.defaults.glossary.ok = '确定';
    alertify.defaults.glossary.cancel = '取消';

    alertify.defaults.theme.ok = "btn btn-primary";
    alertify.defaults.theme.cancel = "btn btn-light";
    alertify.defaults.notifier.delay = 3;
    alertify.defaults.notifier.position = 'top-center';
    alertify.hxDialog || alertify.dialog('hxDialog', function () {
        /**
         * Returns `true` if the passed value is a JavaScript Array, `false` otherwise.
         *
         * @param {Object} target The target to test.
         * @return {Boolean}
         * @method
         */
        var isArray = ('isArray' in Array) ? Array.isArray : function (value) {
            return toString.call(value) === '[object Array]';
        }
        /**
         * 判断给定值是否为空
         * @param {any} value 要判断的值
         * @param {Boolean} allowEmptyString 是否允许空字符串
         * @returns {Boolean} true为空，false不为空
         */
        function isEmpty(value, allowEmptyString) {
            return (value === undefined || value === null) || (!allowEmptyString ? value === '' : false) || (isArray(value) && value.length === 0);
        }
        /**
         * 判断给定值是否是字符串
         * @param {any} value 要验证的值
         * @returns {Boolean} true代表是字符串，false代表不是字符串
         */
        function isString(value) {
            return typeof value === 'string';
        }
        /**
         * 判断给定制是否是对象
         * @param {any} value 要验证的值
         * @returns {Boolean} true代表是对象，false代表不是对象
         */
        function isObject(value) {
            if (toString.call(null) === '[object Object]') {
                //在这里检查ownerDocument以排除DOM节点
                return value !== null && toString.call(value) === '[object Object]' && value.ownerDocument === undefined;
            } else {
                return toString.call(value) === '[object Object]';
            }
        }
        /**
        * 检查给定元素是否具有样式类。
        * @param {Element} element - 要检查的元素
        * @param {string} value - 要搜索的类。
        * @returns {boolean} 如果找到样式类，则返回“true”。
        */
        function hasClass(element, value) {
            return element.classList ? element.classList.contains(value) : element.className.indexOf(value) > -1;
        }
        /**
         * 将样式类添加到给定元素。
         * @param {Element} element -目标元素。.
         * @param {string|string[]} value - 要添加的类。
         */
        function addClass(element, value) {
            if (isEmpty(value) && !element) {
                return;
            }
            var classArr = [],
                classStr;
            if (isString(value)) {
                classStr = value;
                classArr = value.split(' ');
            } else if (isArray(value)) {
                classStr = value.join(' ');
                classArr = value;
            }
            if (element.classList) {
                if (isArray(classArr)) {
                    classArr.forEach(function (r) {
                        element.classList.add(r);
                    });
                }
                return;
            }

            var className = element.className.trim();
            if (!className) {
                element.className = classStr;
            } else if (className.indexOf(classStr) < 0) {
                if (isArray(classArr)) {
                    classArr.forEach(function (r) {
                        if (className.indexOf(r) < 0)
                            element.className = "".concat(className, " ").concat(r);
                    });
                }

            }
        }
        /**
         * 从给定元素中删除类。
         * @param {Element} element - 目标元素。.
         * @param {string|string[]} value - 要移除的类。
         */
        function removeClass(element, value) {
            if (isEmpty(value)) {
                return;
            }
            var classArr = [],
                classStr;
            if (isString(value)) {
                classStr = value;
                classArr = value.split(' ');
            } else if (isArray(value)) {
                classStr = value.join(' ');
                classArr = value;
            }
            if (element.classList) {
                if (isArray(classArr)) {
                    classArr.forEach(function (r) {
                        element.classList.remove(r);
                    });
                }
                return;
            }
            var className = element.className || '';
            if (isArray(classArr)) {
                classArr.forEach(function (r) {
                    if (className.indexOf(r) >= 0)
                        element.className = element.className.replace(r, '');
                });
            }
        }
        /**
         * 添加或删除给定元素中的类。
         * @param {Element} element - 目标元素。.
         * @param {string|string[]} value - 要切换的类。
         * @param {boolean} added - Add only.
         */

        function toggleClass(element, value, added) {
            if (isEmpty(value)) {
                return;
            }
            if (added) {
                addClass(element, value);
            } else {
                removeClass(element, value);
            }
        }
        var on = (function () {
            if (document.addEventListener) {
                return function (el, event, fn, useCapture) {
                    el.addEventListener(event, fn, useCapture === true);
                };
            } else if (document.attachEvent) {
                return function (el, event, fn) {
                    el.attachEvent('on' + event, fn);
                };
            }
        }());
        var off = (function () {
            if (document.removeEventListener) {
                return function (el, event, fn, useCapture) {
                    el.removeEventListener(event, fn, useCapture === true);
                };
            } else if (document.detachEvent) {
                return function (el, event, fn) {
                    el.detachEvent('on' + event, fn);
                };
            }
        }());
        var templates = {
            back: `<span class="hx-i-back" style="cursor: pointer;"></span> `
        };
        function createBack(instance) {
            var backDiv = document.createElement('div'),
                first = instance.elements.dialog.firstChild,
                custom = {};
            backDiv.style.display = 'none';
            backDiv.style.position = 'absolute';
            backDiv.style.left = '4px';
            backDiv.style.margin = '-10px 0 0 24px';
            backDiv.style.zIndex = '2';
            backDiv.innerHTML = templates.back;
            custom.back = backDiv.firstChild;
            instance.elements.dialog.insertBefore(backDiv, first);
            instance.elements.custom = custom;
            instance.__internal.backhanlder = instance.settings.onback;
        }
        function bindBackEvent(instance) {
            var c = instance.elements.custom;
            // 返回功能
            if (instance.get('backable')) {
                if (typeof instance.__internal.backhanlder === 'function' && !instance.__internal.initback) {
                    on(c.back, 'click', instance.__internal.backhanlder);
                    instance.__internal.initback = true;
                }
            }
        }
        function unbindBackEvent(instance) {
            var c = instance.elements.custom;
            // 返回功能
            if (instance.get('backable')) {
                if (typeof instance.__internal.backhanlder === 'function') {
                    off(c.back, 'click', instance.__internal.backhanlder);
                    instance.__internal.initback = false;
                }
            }
        }
        function adjustWidth(instance, width) {
            var w = parseFloat(width),
                regex = /(\d*\.\d+|\d+)%/,
                dialog = instance.elements.dialog;
            if (!isNaN(w)) {
                if (('' + width).match(regex)) {
                    dialog.style.width = width;
                } else {
                    dialog.style.width = w + 'px';
                }
            }
        }
        function adjustMaxWidth(instance, maxWidth) {
            var mw = parseFloat(maxWidth),
                regex = /(\d*\.\d+|\d+)%/,
                dialog = instance.elements.dialog;
            if (!isNaN(mw)) {
                if (('' + maxWidth).match(regex)) {
                    dialog.style.maxWidth = maxWidth;
                } else {
                    dialog.style.maxWidth = mw + 'px';
                }
            } else {
                dialog.style.maxWidth = 'unset';
            }
        }
        function adjustPosition(instance, center) {
            var dialog = instance.elements.dialog,
                autoSet = instance.get('autoReset');
            if (center) {
                if (autoSet || !instance.__internal.initCenter) {
                    var bodyHeight = document.body.clientHeight,
                        dialogHeight = dialog.clientHeight,
                        offsetTop = dialog.offsetTop,
                        height = bodyHeight / 2 - dialogHeight / 2 - offsetTop;
                    dialog.style.top = height + 'px';
                    instance.__internal.initCenter = true;
                }
            } else {
                dialog.style.top = '';
                instance.__internal.initCenter = false;
            }
        }
        return {
            main: function (content) {
                this.setContent(content);
                adjustPosition(this, this.get('center'));
            },
            build() {
                adjustWidth(this, this.get('width'));
                adjustMaxWidth(this, this.get('maxWidth'));
                createBack(this);
            },
            hooks: {
                onclose: function () {
                    var dialog = this.elements.dialog;
                    unbindBackEvent(this);
                    this.__internal.offset = {
                        top: dialog.style.top,
                        left: dialog.style.left
                    }
                }
            },
            prepare: function () {
                bindBackEvent(this);
                var dialog = this.elements.dialog,
                    offset = this.__internal.offset;
                if (offset) {
                    dialog.style.top = offset.top;
                    dialog.style.left = offset.left;
                }
            },
            setup: function () {
                return {
                    buttons: [
                        {
                            text: alertify.defaults.glossary.cancel,
                            key: 27,
                            //invokeOnClose: true,
                            className: alertify.defaults.theme.cancel,
                        },
                        {
                            text: alertify.defaults.glossary.ok,
                            key: 13,
                            className: alertify.defaults.theme.ok,
                        }
                    ],
                    focus: {
                        element: function () {
                            var input = this.elements.body.querySelector(this.get('selector'));
                            return input || 0;
                        },
                        select: true
                    },
                    options: {
                        autoReset: false,
                        closableByDimmer: false,
                        title: '&nbsp;',
                        maximizable: false,
                        resizable: false,
                        padding: false
                    }
                };
            },
            settings: {
                labels: null,
                onback: null,
                onok: null,
                oncancel: null,
                backable: false,
                selector: undefined,
                width: 320,
                maxWidth: null,
                center: false
            },
            setBack: function (back) {
                if (back === true) {
                    this.elements.custom.back.parentElement.style.display = 'block';
                    bindBackEvent(this);
                } else {
                    this.elements.custom.back.parentElement.style.display = 'none';
                    unbindBackEvent(this);
                }
            },
            settingUpdated: function (key, oldValue, newValue) {
                switch (key) {
                    case 'backable':
                        this.setBack(newValue);
                        break;
                    case 'onback':
                        if (typeof newValue === 'function') {
                            this.__internal.backhanlder = newValue;
                            bindBackEvent(this);
                        } else {
                            this.__internal.backhanlder = oldValue;
                            unbindBackEvent(this);
                        }
                    case 'width':
                        adjustWidth(this, newValue);
                        break;
                    case 'maxWidth':
                        adjustMaxWidth(this, newValue);
                        break;
                    case 'center':
                        this.__internal.initCenter = false;
                        adjustPosition(this, newValue);
                        break;
                    case 'labels':
                        if ('ok' in newValue && this.__internal.buttons[1].element) {
                            var text = this.__internal.buttons[1].text;
                            if (isString(newValue.ok)) {
                                text = newValue.ok;
                            } else if (isObject(newValue.ok)) {
                                text = newValue.ok.text;
                                var newClassName = newValue.ok.className;
                                if (!isEmpty(newClassName)) {
                                    var btnElement = this.__internal.buttons[1].element,
                                        oldClassName = this.__internal.buttons[1].className;
                                    removeClass(btnElement, oldClassName);
                                    addClass(btnElement, newClassName);
                                    this.__internal.buttons[1].className = isArray(newClassName) ? newClassName.join(' ') : newClassName;
                                }
                            }
                            this.__internal.buttons[1].text = text;
                            this.__internal.buttons[1].element.innerHTML = text;
                        }
                        if ('cancel' in newValue && this.__internal.buttons[0].element) {
                            var text = this.__internal.buttons[0].text;
                            if (isString(newValue.cancel)) {
                                text = newValue.cancel;
                            } else if (isObject(newValue.cancel)) {
                                text = newValue.ok.text;
                                var newClassName = newValue.cancel.className;
                                if (!isEmpty(newClassName)) {
                                    var btnElement = this.__internal.buttons[0].element,
                                        oldClassName = this.__internal.buttons[0].className;
                                    removeClass(btnElement, oldClassName);
                                    addClass(btnElement, newClassName);
                                    this.__internal.buttons[0].className = isArray(newClassName) ? newClassName.join(' ') : newClassName;
                                }
                            }
                            this.__internal.buttons[0].text = text;
                            this.__internal.buttons[0].element.innerHTML = text;
                        }
                        break;
                }
            },
            callback: function (closeEvent) {
                var returnValue;
                switch (closeEvent.index) {
                    case 0:
                        if (typeof this.get('oncancel') === 'function') {
                            returnValue = this.get('oncancel').call(this, closeEvent);
                            if (typeof returnValue !== 'undefined') {
                                closeEvent.cancel = !returnValue;
                            }
                        }
                    case 1:
                        if (typeof this.get('onok') === 'function') {
                            returnValue = this.get('onok').call(this, closeEvent);
                            if (typeof returnValue !== 'undefined') {
                                closeEvent.cancel = !returnValue;
                            }
                        }
                        break;
                        break;
                }
            }
        };
    });
}