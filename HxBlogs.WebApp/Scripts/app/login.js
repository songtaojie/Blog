"use strict";
var HxAccount = function () {
    /**
    * 是否显示登录页
    * @param {boolean} showLogin true显示登录页，隐藏注册账号页
    */
    var switchShow = function (showLogin) {
        if (showLogin) {
            $('.hx-login').removeAttr('hidden');
            $('.hx-register').attr('hidden', '');
        } else {
            $('.hx-login').attr('hidden', '');
            $('.hx-register').removeAttr('hidden');
        }
    };
    return {
        switchShow,
        registerShow:null,
        beforeLogin: function (a, b) {
            if (HxCore.blockUI)
                HxCore.blockUI({ label: '登陆中...' });
        },
        finishLogin: function (op, success, r) {
        },
        afterLogin: function (data, textStatus, jqXHR) {
            HxCore.ajaxSuccess(jqXHR, function (d) {
                window.location.href = d && decodeURIComponent(d) || "/";
                if (HxCore.unblockUI)
                    HxCore.unblockUI();
            }, function () {
                $('#img').attr('src', $('#img').attr('src') + 1);
                if (HxCore.unblockUI)
                    HxCore.unblockUI();
            });
        },
        doFailure(data, err) {
            if (HxCore.unblockUI)
                HxCore.unblockUI();
            HxCore.ajaxError(data);
        },
        init: function (keypress) {
            $('.login-form').validate({
                errorElement: 'span',
                errorClass: 'text-danger',
                focusInvalid: false,
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
                    $('.alert-error', $('.login-form')).show();
                },
                //highlight: function (element) {
                //    $(element).closest('.form-row').addClass('error'); 
                //},
                success: function (error) {
                    //error.closest('.form-row').removeClass('error');
                    error.remove();
                }
            });
            if (keypress) {
                $(document).keypress(function (e) {
                    if (e.which === 13) {
                        $('.login-form').submit();
                        return false;
                    }
                });
            }
            $('#btn2register').click(function () {
                switchShow();
                if (HxCore.isFunction(HxAccount.registerShow)) {
                    HxAccount.registerShow();
                }
            });
            $('#btnBack').click(function () {
                switchShow(true);
            });
        },
        //如果按钮时禁用状态，禁止提交
        beforeRegister: function () {
            var btnReg = $('#btn-Reg');
            if (btnReg.hasClass('disabled')) {
                return false;
            }
            btnReg.addClass('disabled');
        },
        afterRegister: function (r) {
            HxCore.ajaxSuccess(r, function (data) {
                window.location.href = data && decodeURIComponent(data) || "/";
            });
        },
        //修改按钮状态为可用
        finishRegister: function () {
            var btnReg = $('#btn-Reg');
            if (btnReg.hasClass('disabled')) {
                btnReg.removeClass('disabled');
            }
        },
    };
}();
