if (!$.blockUI) { throw new Error("HxLoad requires blockUI"); }
; (function ($, window) {
    "use strict";
    var hxLoad = {

    };
    var blockUI = function (opt) {
        opt = $.extend(true, {}, opt);
        var me = this,
            loadCls = 'hx-loading-fading-circle',
            html = '';
        var mode = (opt.mode || '').toLowerCase();
        if (mode == 'bounce' || mode == 'b') {
            loadCls = 'hx-loading-bounce';
            html = `<div class="${loadCls}">
                        <div class="hx-loading-child bounce1"></div>
                        <div class="hx-loading-child bounce2"></div>
                        <div class="hx-loading-child bounce3"></div>
                        <span style='display:block;'>&nbsp;&nbsp;${opt.message ? opt.message : ''}
                    </div>`;
        } else if (mode == 'circle' || mode == 'c') {
            loadCls = 'hx-loading-circle';
            html += `<div class="${loadCls}">`;
            for (var i = 1; i <= 12; i++) {
                html += `<div class="circle${i} hx-loading-child"></div>`;
            }
            html += `<span style='display:block;'>&nbsp;&nbsp;${opt.message ? opt.message : ''}`;
            html += '</div>';
        } else {
            html += `<div class="${loadCls}">`;
            for (var i = 1; i <= 12; i++) {
                html += `<div class="fadecircle${i} hx-loading-circle"></div>`;
            }
            html += `<span style='display:block;'>&nbsp;&nbsp;${opt.message ? opt.message : ''}`;
            html += '</div>';
        }

        if (opt.target) {
            var el = $(opt.target);
            if (el.height() <= ($(window).height())) {
                opt.cenrerY = true;
            }
            el.block({
                message: html,
                baseZ: opt.zIndex ? opt.zIndex : 1000,
                centerY: opt.cenrerY !== undefined ? opt.cenrerY : false,
                css: {
                    top: '10%',
                    border: '0',
                    padding: '0',
                    backgroundColor: 'none'
                },
                overlayCSS: {
                    backgroundColor: opt.overlayColor ? opt.overlayColor : '#555',
                    opacity: opt.boxed ? 0.05 : 0.1,
                    cursor: 'wait'
                }
            });
        } else { // page blocking
            $.blockUI({
                message: html,
                baseZ: opt.zIndex ? opt.zIndex : 1000,
                css: {
                    border: '0',
                    padding: '0',
                    backgroundColor: 'none'
                },
                overlayCSS: {
                    backgroundColor: opt.overlayColor ? opt.overlayColor : '#555',
                    opacity: opt.boxed ? 0.05 : 0.1,
                    cursor: 'wait'
                }
            });
        }
    },
        unblockUI = function(el) {
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
        };
    
    window.HxLoad = $.extend(true, hxLoad, {
        blockUI(option) {
            blockUI(option);
        },
        unblockUI(el) {
            unblockUI(el);
        }
    });
})(jQuery, window);