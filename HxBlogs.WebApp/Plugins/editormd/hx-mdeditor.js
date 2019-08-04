$(function () {
    editormd("editor", {
        //width: "90%",
        height: 740,
        path: '/plugins/editormd/lib/',
        //theme: "default",
        //previewTheme: "default",
        placeholder: '开始编写博客!',
        //editorTheme: "pastel-on-dark",
        //editorTheme: "default",
        //markdown: md,
        codeFold: true,
        toolbarAutoFixed: true,
        //syncScrolling : false,
        saveHTMLToTextarea: true,    // 保存 HTML 到 Textarea
        searchReplace: true,
        //watch : false,                // 关闭实时预览
        htmlDecode: "style,script,iframe|on*",            // 开启 HTML 标签解析，为了安全性，默认不开启
        //toolbar  : false,             //关闭工具栏
        //previewCodeHighlight : false, // 关闭预览 HTML 的代码块高亮，默认开启
        emoji: false,
        taskList: true,
        tocm: true,         // Using [TOCM]
        tex: true,                   // 开启科学公式TeX语言支持，默认关闭
        flowChart: true,             // 开启流程图支持，默认关闭
        sequenceDiagram: true,       // 开启时序/序列图支持，默认关闭,
        //dialogLockScreen : false,   // 设置弹出层对话框不锁屏，全局通用，默认为true
        //dialogShowMask : false,     // 设置弹出层对话框显示透明遮罩层，全局通用，默认为true
        //dialogDraggable : false,    // 设置弹出层对话框不可拖动，全局通用，默认为true
        //dialogMaskOpacity : 0.4,    // 设置透明遮罩层的透明度，全局通用，默认值为0.1
        //dialogMaskBgColor : "#000", // 设置透明遮罩层的背景颜色，全局通用，默认为#fff
        imageUpload: true,
        imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
        imageUploadURL: "/file/upload",
        uploadName:'upload',
        toolbarIcons: function () {
            // Or return editormd.toolbarModes[name]; // full, simple, mini
            // Using "||" set icons align right.
            var w = window.innerWidth;
            if (w < 768) {
                return ["undo", "redo", "|", "image", "|", "||",
                     "preview", "fullscreen", "clear"];
            }
            return ["undo", "redo", "|", "bold",  "italic", "|",
                "h1", "h2", "h3",  "|", "list-ul", "list-ol","|", "image", "code",
                "table", "||",
                "watch","preview", "fullscreen", "clear", "help"];
        },
        onfullscreen() {
            $('header,div.form-row:not(.hx-editor)').hide();
            //this.toolbar.css('margin-top', 'auto');
            //this.resize();
        },
        onfullscreenExit() {
            $('header,div.form-row:not(.hx-editor)').show();
            this.width('100%');
        },
        onload: function () {
            window._HxEditor = this;
            $('div[hidden]').removeAttr('hidden');
            var me = this,
                state = me.state,
                editor = me.editor,
                toolbar = me.toolbar,
                settings = me.settings,
                $window = $(window),
                w = $window.width();
            var autoFixedHandle = function () {
                var top = $window.scrollTop();
                if (!settings.toolbarAutoFixed) {
                    return false;
                }
                if (top - editor.offset().top > -8 && top < editor.height() && !state.fullscreen) {

                    toolbar.css({
                        'margin-top': '3.25rem',
                        position: "fixed",
                        width: editor.width() + "px",
                        left: ($window.width() - editor.width()) / 2 + "px"
                    });
                }
                else {
                    toolbar.css({
                        'margin-top': 'auto',
                        position: "absolute",
                        width: "100%",
                        left: 0
                    });
                }
            };
            if (!state.fullscreen && !state.preview && settings.toolbar && settings.toolbarAutoFixed) {
                $(window).bind("scroll", autoFixedHandle);
            }
            var smallScreen = function () {
                me.unwatch();
                me.config('lineNumbers', false);
            };
            if (w < 768) {
                smallScreen();
            }
            $window.resize(function () {
                var width = $window.width();
                if (width < 768) {
                    smallScreen();
                }
                if (state.fullscreen) {
                    editor.css({
                        width: $window.width(),
                        height: $window.height()
                    });
                    me.resize();
                }
            });
        }
    });
});