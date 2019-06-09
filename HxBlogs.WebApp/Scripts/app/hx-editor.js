$(function () {
    $('input:checkbox:not(.blog-tag)').change(function () {
        var $me = $(this);
        if ($me.is(':checked')) {
            $me.val('true').attr('checked', '');
        } else {
            $me.val('false').removeAttr('checked');
        }
    });
    $('#blogTag').on('change', 'input:checkbox', function (a, b, c) {
        var $me = $(this),
            tagObj = {},
            $label = $me.next(),
            text = $.trim($label.text()),
            guid;
        tagObj[$me.val()] = text;
        if ($me.is(':checked')) {
            guid = HxCore.guid();
            $me.prop('checked', true).data('id', guid);
            Edit.addTag({
                id: $me.attr('id'),
                guid: guid,
                text: text
            });
            Edit.doPersonTag(tagObj, true);
        } else {
            Edit.doPersonTag(tagObj);
            $me.prop('checked', false);
            guid = $me.data('id');
            $('#' + guid).remove();
        }
    });
    $('.btn-save').click(function () {
        if (Edit.validate()) {
            var $me = $(this),
                data = $('form').serializeArray(),
                action = $('form').attr('action') || 'save';
            data.push({ name: 'IsPublish', value: $me.val() });
            var content = data.find(d => { return d.name === 'Content'; }),
                contentHtml = data.find(d => { return d.name === 'ContentHtml'; }),
                d ,
                hd;
            if (Edit.isMd()) {
                d = window._HxEditor.getMarkdown();
                hd = window._HxEditor.getHTML();
            } else {
                d = window._HxEditor.getData();
                hd = window._HxEditor.getData();
            }
            if (content) {
                content.value = d;
            } else {
                data.push({ name: 'Content', value: d });
            }
            if (contentHtml) {
                contentHtml.value = hd;
            } else {
                data.push({ name: 'ContentHtml', value: hd });
            }
            HxCore.ajax(action, {
                button: '.btn-save',
                data: data,
                maskTarget: true,
                success: function (d) {
                    debugger
                }
            });
        }
    });
    var Edit = {
        isMd() {
            return window._HxEditor.getMarkdown ? true : false;
        },
        validate() {
            var $form = $('form'),
                allField = $form.find('input[name][required],select[required]'),
                result = true;
            allField.each(function (index, el) {
                var field = $(el),
                    isEmpty = false;
                if (field.is('input')) {
                    if ($.isEmpty(field.val())) {
                        isEmpty = true;
                    }
                } else if (field.is('select')) {
                    if (field.val() === '-1') {
                        isEmpty = true;
                    }
                }
                if (isEmpty) {
                    var prompt = field.data('prompt');
                    HxCore.remindError(`请输入${prompt}!`);
                    result = false;
                    return false;
                }
            });
            if (result) {
                var isMdEmpty = Edit.isMd() && HxCore.isEmpty(window._HxEditor.getMarkdown()),
                    isCkEmpty = !Edit.isMd() && HxCore.isEmpty(window._HxEditor.getData());
                if (isMdEmpty || isCkEmpty) {
                    HxCore.remindError('请输入博客内容!');
                    result = false;
                }
            }
            return result;
        },
        doPersonTag(text, add) {
            var $tag = $('input[name=PersonTags]'),
                tagVal = $tag.val(),
                tagObj = {},
                result = false;
            if (!$.isEmpty(tagVal)) {
                tagObj = JSON.parse(tagVal);
            }
            if (add) {
                var count = $tag.data('count'),
                    exist = false;
                if ($.isString(text)) {
                    text = $.trim(text);
                    $.each(tagObj, (key, value) => {
                        if ($.trim(value) == text) {
                            exist = true;
                            return false;
                        }
                    });
                    count++;
                    if (!exist) {
                        result = true;
                        tagObj['newData' + count] = text;
                        $tag.data('count', count);
                    }
                } else if ($.isObject(text)) {
                    $.extend(tagObj, text);
                }
                if (!$.isEmptyObject(tagObj)) {
                    $tag.val(JSON.stringify(tagObj));
                }
            } else {
                if ($.isString(text)) {
                    text = $.trim(text);
                    $.each(tagObj, (key, value) => {
                        if ($.trim(value) == text) {
                            delete tagObj[key];
                            result = true;
                            return false;
                        }
                    });
                } else if ($.isObject(text)) {
                    $.each(tagObj, (key, value) => {
                        if ($.trim(value) == $.trim(text[key])) {
                            delete tagObj[key];
                            result = true;
                            return false;
                        }
                    });
                }
                $tag.val(JSON.stringify(tagObj));
            }
            return result;
        },
        addTag(tagObj) {
            var $div = $('<div class="blog-tag-box"></div>'),
                $span = $('<span class="tag" contenteditable = "true"></span>'),
                $i = $('<i class="fa fa-times"></i>');
            $div.append($span).append($i).insertBefore($('#btn-addtag'));
            //$div.find('i.fa-times').click(function () {
               
            //});
            if (HxCore.isObject(tagObj) && tagObj.hasOwnProperty('id') && tagObj.hasOwnProperty('text')) {
                $div.attr('id', tagObj.guid).data('id', tagObj.id);
                $span.text($.trim(tagObj.text)).attr('contenteditable', 'false');
            } else {
                //直接放在span元素上注册会出现错误即$span.blur,所以通过这种方式注册
                $div.one('blur', 'span.tag', function () {
                    var $span = $(this),
                        text = $.trim($span.text()),
                        $parent = $span.parent();
                    if ($.isEmpty(text)) {
                        $parent.remove();
                    } else {
                        $span.attr('contenteditable', 'false');
                        if (!Edit.doPersonTag(text, true)) {
                            $parent.remove();
                        }
                    }
                });
                $span.focus().keydown(function (e) {
                    var code = (e.keyCode ? e.keyCode : e.which);
                    if (code === 13) {
                        var $span = $(e.target),
                            text = $.trim($span.text()),
                            $parent = $span.parent();
                        if ($.isEmpty(text)) {
                            $parent.remove();
                        } else {
                            $span.blur();
                        }
                    }
                });
            }
        }
    };
    $('#btn-addtag').click(function () {
        Edit.addTag();
    });
    $('#person-tags').on('click', 'i.fa-times', function () {
        var $me = $(this),
            $p = $me.parent(),
            id = $p.data('id'),
            $prev = $me.prev();
        $p.remove();
        if (!HxCore.isEmpty(id)) {
            $('#' + id).prop('checked', false);
        }
        Edit.doPersonTag($prev.text());
    });
});