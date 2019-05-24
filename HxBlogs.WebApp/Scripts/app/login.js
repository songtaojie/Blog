"use strict";
var Login = function () {
    return {
        beforeLogin: function (a, b) {
            HxCore.blockUI({label:'登陆中...'});
        },
        finishLogin: function (op, success, r) {
        },
        afterLogin: function (data, textStatus, jqXHR) {
            HxCore.ajaxSuccess(jqXHR, function (d) {
                window.location.href = d && decodeURIComponent(d) || "/";
                HxCore.unblockUI();
            }, function () {
                $('#img').attr('src', $('#img').attr('src') + 1);
                HxCore.unblockUI();
            });
        },
        doFailure(data, err) {
            HxCore.unblockUI();
            HxCore.ajaxError(data);
        },
        init: function () {
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
                            url: 'admin/account/checkcode',
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
            $(document).keypress(function (e) {
                if (e.which === 13) {
                    $('.login-form').submit();
                    return false;
                }
            });
        }
    };
}();
$(document).ready(function (e) {
    Login.init();
    if (window.history && window.history.pushState) {
        $(window).on('popstate', function () {
            window.history.pushState('forward', null, '#');
            window.history.forward(1);
            location.replace(document.referrer);//刷新
        });
        window.history.pushState('forward', null, '#');
        window.history.forward(1);
    }
});