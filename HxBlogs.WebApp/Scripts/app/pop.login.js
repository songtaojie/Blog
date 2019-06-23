; $(function () {
    var _hxDialog;
    hxAccount.login.init({
        enter:'.hx-login',
        oninitend: function () {
            var me = this;
            $('#btn2register').click(function (e) {
                e.preventDefault();
                var loginShow = me.switchShow();
                if (!loginShow && _hxDialog) {
                    _hxDialog.set('backable', true);
                }
            });
        }
    });
    hxAccount.register.init('.hx-register');
    $.extend(hxAccount, {
        showLogin: function () {
            _hxDialog = alertify.hxDialog($('#login').removeAttr('hidden')[0]).
                set({
                    onback: function () {
                        _hxDialog.set('backable', false);
                        hxAccount.switchShow(true);
                    },
                    selector: 'input[name="UserName"]',
                    onclose: function () {
                        hxAccount.switchShow(true);
                        _hxDialog.set('backable', false);
                        if (hxAccount.login.validator)
                            hxAccount.login.validator.resetForm();
                    }
                });
        }
    });
});