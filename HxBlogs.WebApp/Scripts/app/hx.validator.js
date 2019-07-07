$(function () {
    $.validator.addMethod("checkNickName", function (value, element, params) {
        var regular = /^([a-zA-Z]|[\u4E00-\u9FA5])([a-zA-Z0-9]|[\u4E00-\u9FA5]|[_]){1,21}$/;
        return this.optional(element) || (regular.test(value));
    }, "昵称由字母、数字、下划线和中文组成，以中文或字母开头");
    $.validator.addMethod("checkPassWord", function (value, element, params) {
        var regular = /^.*(?=.{6,16})(?=.*\d)(?=.*[A-Z]{1,})(?=.*[a-z]{1,})(?=.*[.!@#$%^&*]).*$/;
        return this.optional(element) || (regular.test(value));
    }, "密码必须包含数字、大小写字母、和一个特殊符号,且长度为6~16");
    $.validator.addMethod("checkRealName", function (value, element, params) {
        var regular = /^[\u4E00-\u9FA5\uf900-\ufa2d·s]{2,20}$/;
        return this.optional(element) || (regular.test(value));
    }, "请输入正确的姓名!");
    $.validator.addMethod("checkMobile", function (value, element, params) {
        var regular = /^1(3|4|5|6|7|8|9)\d{9}$/;
        return this.optional(element) || (regular.test(value));
    }, "请输入正确的手机号!");
    $.validator.addMethod("checkIdCard", function (value, element, params) {
        return this.optional(element) || (hxCore.validateIdCard(value));
    }, "请输入正确的身份证号!");
    $.validator.addMethod("checkQQ", function (value, element, params) {
        var regular = /^[1-9]\d{4,10}$/;
        return this.optional(element) || (regular.test(value));
    }, "请输入正确的QQ号!");
    $.validator.addMethod("checkWeChat", function (value, element, params) {
        var regular = /^[a-zA-Z]([-_a-zA-Z0-9]{5,19})+$/;
        return this.optional(element) || (regular.test(value));
    }, "请输入正确的微信号!");
});