
; (function ($, window) {
    "use strict";
    var hxCore = window.hxCore;
    var loginValid = function (instance) {
        var $form = $('.hx-login form');
        instance.validator = $form.validate({
            errorElement: 'span',
            errorClass: 'text-danger',
            //提交表单后，（第一个）未通过验证的表单获得焦点
            focusInvalid: true,
            //当未通过验证的元素获得焦点时，移除错误提示
            //focusCleanup: true,
            rules: {
                UserName: { required: true },
                PassWord: { required: true },
                ValidateCode: {
                    required: true,
                    remote: {
                        url: '/admin/account/checkcode',
                        type: 'post',
                        data: {
                            code: function () {
                                $('input.input-validate').val();
                            }
                        },
                    }
                }
            },
            messages: {
                UserName: '用户名/邮箱不能为空!',
                PassWord: '密码不能为空!',
                ValidateCode: {
                    required: '验证码不能为空!',
                    remote: '验证码不正确!'
                }
            },
            errorPlacement: function (error, element) {
                error.addClass('small').insertAfter(element.closest('.input-group'));
            },
            ////display error alert on form submit
            invalidHandler: function (event, validator) {
                $('.alert-error', $form).show();
            },
            //highlight: function (element) {
            //    $(element).closest('.form-row').addClass('error'); 
            //},
            success: function (error) {
                //error.closest('.form-row').removeClass('error');
                error.remove();
            }
        });
        return instance;
    }
    var login = {
        init: function (options) {
            var opt = {};
            if (options === true || options === false || hxCore.isString(options)) {
                opt.enter = options;
            } else {
                opt = $.extend(true, opt, options);
            }
            loginValid(this);
            if (opt.enter === true) {
                $(document).keypress(function (e) {
                    if (e.which === 13) {
                        $('.hx-login form').submit();
                        return false;
                    }
                });
            } else if (hxCore.isString(opt.enter)) {
                $(opt.enter).keypress(function (e) {
                    if (e.which === 13) {
                        $('.hx-login form').submit();
                        return false;
                    }
                });
            }
            if (hxCore.isFunction(opt.oninitend)) {
                opt.oninitend.call(window.hxAccount);
            }
            return this;
        },
        begin: function (a, b) {
            var sumitBtn = $('.hx-login button[type=submit]');
            if (sumitBtn.hasClass('disabled')) {
                return false;
            }
            sumitBtn.addClass('disabled');
            if (hxCore.blockUI)
                hxCore.blockUI({ label: '登陆中...' });
        },
        finish: function (op, success, r) {
            if (hxCore.unblockUI)
                hxCore.unblockUI();
            var sumitBtn = $('.hx-login button[type=submit]');
            if (sumitBtn.hasClass('disabled')) {
                sumitBtn.removeClass('disabled');
            }
        },
        success: function (data, textStatus, jqXHR) {
            hxCore.ajaxSuccess(jqXHR, function (d) {
                window.location.href = d && decodeURIComponent(d) || "/";
                if (hxCore.unblockUI)
                    hxCore.unblockUI();
            }, function (data, result) {
                hxCore.remindError(data);
                $('#img').attr('src', $('#img').attr('src') + 1);
                
            });
        },
        failure(data, err) {
            hxCore.ajaxError(data);
        }
    };

    $.validator.addMethod("checkName", function (value, element, params) {
        var regular = /^([a-zA-Z]|[\u4E00-\u9FA5])([a-zA-Z0-9]|[\u4E00-\u9FA5]|[_]){4,31}$/;
        return this.optional(element) || (regular.test(value));
    }, "用户名由字母、数字、下划线和中文组成，以中文或字母开头，且长度为4~30");
    var registerValid = function (instance) {
        var $form = $('.hx-register form');
        instance.validator = $form.validate({
            errorElement: 'span',
            errorClass: 'text-danger',
            //提交表单后，（第一个）未通过验证的表单获得焦点
            focusInvalid: true,
            //当未通过验证的元素获得焦点时，移除错误提示
            //focusCleanup: true,
            rules: {
                UserName: {
                    required: true,
                    checkName: true,
                    remote: {
                        url: '/admin/account/checkusername',
                        type: 'post',
                        data: {
                            userName: function () {
                                $('input[name="UserName"]').val();
                            }
                        },
                    }
                },
                Email: {
                    required: true,
                    email: true,
                    remote: {
                        url: '/admin/account/checkemail',
                        type: 'post',
                        data: {
                            email: function () {
                                $('input[name="Email"]').val();
                            }
                        },
                    }
                },
                PassWord: { required: true },
                PwdConfirm: {
                    required: true,
                    equalTo:'input[name="PassWord"]'
                }
            },
            messages: {
                UserName: {
                    required: '用户名不能为空!',
                },
                Email: {
                    required: '邮箱不能为空!',
                },
                PassWord: '密码不能为空!',
                PwdConfirm:{
                    required: '确认密码不能为空!',
                    equalTo:'两次所输密码不一致!'
                }
            },
            errorPlacement: function (error, element) {
                error.addClass('small').insertAfter(element.closest('.input-group'));
            },
            ////display error alert on form submit
            invalidHandler: function (event, validator) {
                $('.alert-error', $form).show();
            },
            //highlight: function (element) {
            //    $(element).closest('.form-row').addClass('error'); 
            //},
            success: function (error) {
                //error.closest('.form-row').removeClass('error');
                error.remove();
            }
        });
        return instance;
    }
    var register = {
        init: function (options) {
            var opt = {};
            if (options === true || options === false || hxCore.isString(options)) {
                opt.enter = options;
            } else {
                opt = $.extend(true, opt, options);
            }
            registerValid(this);
            if (opt.enter === true) {
                $(document).keypress(function (e) {
                    if (e.which === 13) {
                        $('.hx-register form').submit();
                        return false;
                    }
                });
            } else if (hxCore.isString(opt.enter)) {
                $(opt.enter).keypress(function (e) {
                    if (e.which === 13) {
                        $('.hx-register form').submit();
                        return false;
                    }
                });
            }
            if (hxCore.isFunction(opt.oninitend)) {
                opt.oninitend.call(window.hxAccount);
            }
            return this;
        },
        //如果按钮时禁用状态，禁止提交
        begin: function () {
            var submitBtn = $('.hx-register input[type=submit]');
            if (submitBtn.hasClass('disabled')) {
                return false;
            }
            submitBtn.addClass('disabled');
        },
        success: function (r) {
            hxCore.ajaxSuccess(r, function (data) {
                window.location.href = data && decodeURIComponent(data) || "/";
            });
        },
        //修改按钮状态为可用
        finish: function () {
            var submitBtn = $('.hx-register input[type=submit]');
            if (submitBtn.hasClass('disabled')) {
                submitBtn.removeClass('disabled');
            }
        }
    }
    /**
    * 
    * 是否显示登录页
    * @param { boolean } show true显示登录页，隐藏注册账号页
    */
    var switchShow = function (show) {
        var $login = $('.hx-login'),
            $register = $('.hx-register'),
            loginShow = show === true;
        if (loginShow) {
            if (hxAccount.register.validator)
                hxAccount.register.validator.resetForm();
        } else {
            if (hxAccount.login.validator)
                hxAccount.login.validator.resetForm();
        }
        $login.attr('hidden', !loginShow);
        $register.attr('hidden', loginShow);
        return loginShow;
    };
    var HxAccount = function () {
        return {
            switchShow,
            login,
            register
        };
    };
    window.hxAccount = new HxAccount();
}(jQuery, window));

