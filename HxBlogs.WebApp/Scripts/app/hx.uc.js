; Dropzone.autoDiscover = false;
$(function () {
    var uc = {
        $cropper: $('#cropperavatar'),
        clearCropper() {
            if (uc.cropper) uc.cropper.destroy();
            if (uc.dropzone) uc.dropzone.removeAllFiles();
            if (uc.$cropper) uc.$cropper.attr('hidden', 'hidden');
            if (uc.$dropzone) uc.$dropzone.removeAttr('hidden');
        },
        updateAvatar(src) {
            if (!hxCore.isEmpty(src)) {
                var avatars = $('.hx-avatar');
                avatars.find("img").attr("src", src);
            }
        }
    };
    $('.hx-avatar').mouseenter(function () {
        var first = $(this).children().first();
        first.removeClass('hx-hidden').addClass('hx-visiable hx-active');
    }).mouseleave(function () {
        var first = $(this).children().first();
        first.removeClass(['hx-visiable', 'hx-active']).addClass('hx-hidden');
    });
    $('.btn-edit-avatar').click(function () {
        if (uc.hxDialog && uc.hxDialog.show) {
            uc.hxDialog.show();
        } else {
            uc.hxDialog = alertify.hxDialog($('#imageDialog').removeAttr('hidden')[0])
                .set({
                    title: '上传头像',
                    width: 750,
                    center: true,
                    labels: {
                        ok: {
                            text: '上传',
                            className: 'btn btn-success'
                        },
                        cancel: '重新选择'
                    },
                    onok: function () {
                        if (uc.dropzone) uc.dropzone.processQueue();
                        return false;
                    },
                    oncancel: function () {
                        uc.clearCropper();
                        if (uc.dropzone) {
                            $(uc.dropzone.element).trigger('click');
                        }
                        return false;
                    }
                });
        }

    });
    $('.dropzone').dropzone({
        url: "/file/uploadavatar",
        previewsContainer: uc.$cropper[0],
        dictInvalidFileType: '请上传图片文件',
        dictFileTooBig: "文件超出限制. 最大为: {{maxFilesize}}MB.",
        dictMaxFilesExceeded: '只能上传单个文件',
        //dictRemoveFile: '移除',
        paramName: "file", // The name that will be used to transfer the file
        maxFiles: 1,
        maxFilesize: 2, // MB
        acceptedFiles: "image/*",
        //addRemoveLinks: true,
        autoProcessQueue: false,
        thumbnailWidth: 360,
        thumbnailHeight: null,
        previewTemplate: `
        <div class="dz-preview dz-file-preview">
            <div class="dz-details">
            <img data-dz-thumbnail />
            </div>
        </div>`,
        init: function () {
            uc.dropzone = this;
            this.on('addedfile', function (file) {
                var name = file.name || '',
                    ext,
                    lastIndex = name.lastIndexOf('.');
                if (lastIndex >= 0) {
                    name = name.substring(0, lastIndex);
                    ext = name.substring(lastIndex);
                }
                uc.originalFile = {
                    file,
                    name,
                    ext
                };
            });
            this.on('thumbnail', function (file, dataUrl) {
                uc.$dropzone = $(uc.dropzone.element.parentElement).attr('hidden', 'hidden');
                uc.$cropper = uc.$cropper.removeAttr('hidden');
                uc.$cropperimg = uc.$cropper.find('img');
                uc.$cropperimg.on({
                    ready: function (e) {
                        uc.cropper = this.cropper;
                    }
                }).cropper({
                    aspectRatio: 1,
                    viewMode: 1,
                    preview: '.img-preview'
                });
            });
            this.on('success', function (files, r) {
                uc.hxDialog.close();
                uc.updateAvatar(r.Resultdata);
                uc.clearCropper();
            });
            this.on('sending', function (data, xhr, formData) {
                if (uc.cropper) {
                    var previews = uc.cropper.previews;
                    if (previews && previews.length > 0) {
                        var fileName = uc.originalFile.name + '-160x160.png',
                            file = hxCore.dataURLtoFile(previews[0].firstChild.src, fileName);
                        formData.append('file1', file, fileName);
                    }
                }
            });
            this.on('maxfilesexceeded', function (file) {
                if (file && !file.accepted) {
                    uc.dropzone.removeFile(file);
                }
            });
            this.on('error', function (files, r) {
                if (r && !r.Success) {
                    hxCore.remindError(r.Message || '上传失败!');
                }
                uc.clearCropper();
            });
        }
    });

    $('.date-picker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        orientation: 'auto bottom',
        language: 'zh-CN'
    });
    var basic = function () {
        var $form = $('#basicForm');
        function validate() {
            return $form.validate({
                errorElement: 'span',
                errorClass: 'text-danger',
                //提交表单后，（第一个）未通过验证的表单获得焦点
                focusInvalid: true,
                //当未通过验证的元素获得焦点时，移除错误提示
                //focusCleanup: true,
                rules: {
                    NickName: {
                        required: true,
                        minlength: 2,
                        maxlength: 320,
                        checkNickName: true,
                    },
                    RealName: {
                        checkRealName: true
                    },
                    Mobile: {
                        checkMobile: true
                    },
                    QQ: {
                        checkQQ: true
                    },
                    WeChat: {
                        checkWeChat: true
                    }
                },
                messages: {
                    NickName: {
                        required: '昵称不能为空!'
                    }
                },
                errorPlacement: function (error, element) {
                    $(element).next().hide();
                    error.addClass(['small ', 'my-auto']).insertAfter(element);
                },
                ////display error alert on form submit
                invalidHandler: function (event, validator) {
                    $('.alert-error', uc.$basicform).show();
                },
                //highlight: function (element) {
                //    $(element).closest('.form-row').addClass('error');
                //},
                success: function (error, element) {
                    //error.closest('.form-row').removeClass('error');
                    $(error).next().show();
                    error.remove();
                }
            });
        }
        return {
            $form,
            begin: function () {
                var submitBtn = this.$form.find('input[type=submit]');
                if (submitBtn.hasClass('disabled')) {
                    return false;
                }
                submitBtn.addClass('disabled');
            },
            success: function (data, textStatus, jqXHR) {
                hxCore.ajaxSuccess(jqXHR, function (data) {
                    hxCore.remindSuccess('用户基本资料修改成功!');
                });
            },
            failure(r) {
                hxCore.ajaxError(r);
            },
            finish: function () {
                var submitBtn = this.$form.find('input[type=submit]');
                if (submitBtn.hasClass('disabled')) {
                    submitBtn.removeClass('disabled');
                }
            },
            validator: validate()
        }
    };
    var job = function () {
        var $form = $('#jobForm');
        return {
            $form,
            begin: function () {
                var submitBtn = this.$form.find('input[type=submit]');
                if (submitBtn.hasClass('disabled')) {
                    return false;
                }
                submitBtn.addClass('disabled');
            },
            success: function (data, textStatus, jqXHR) {
                hxCore.ajaxSuccess(jqXHR, function (data) {
                    hxCore.remindSuccess('职业技能修改成功!');
                });
            },
            failure(r) {
                hxCore.ajaxError(r);
            },
            finish: function () {
                var submitBtn = this.$form.find('input[type=submit]');
                if (submitBtn.hasClass('disabled')) {
                    submitBtn.removeClass('disabled');
                }
            }
        }
    }
    uc.basic = new basic();
    uc.job = new job();
    window.uc = uc;
});


