using Hx.Framework.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class BasicInfoDTO : IAutoMapper<Model.BasicInfo>
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空!")]
        [RegularExpression("^([a-zA-Z]|[\u4E00-\u9FA5])([a-zA-Z0-9]|[\u4E00-\u9FA5]|[_]){1,21}$",
            ErrorMessage = "昵称由2~20个字符且由字母、数字、下划线和中文组成，以中文或字母开头")]
        public string NickName { get; set; }

        /// <summary>
        /// 真实的名字
        /// </summary>
        [RegularExpression("^[\u4E00-\u9FA5\uf900-\ufa2d·s]{2,20}$", ErrorMessage = "请输入正确的姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string CardId
        {
            set; get;
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday
        {
            set; get;
        }

        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(1)]
        public string Gender
        {
            set; get;
        }
        /// <summary>
        /// 用户的QQ
        /// </summary>
        [RegularExpression("^[1-9]\\d{4,10}$", ErrorMessage = "请输入正确的QQ号!")]
        public string QQ
        {
            get; set;
        }
        /// <summary>
        /// 用户微信号
        /// </summary>
        [RegularExpression("^[a-zA-Z]([-_a-zA-Z0-9]{5,19})+$", ErrorMessage = "请输入正确的微信号!")]
        public string WeChat
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set; get;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone
        {
            set; get;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression("^1(3|4|5|6|7|8|9)\\d{9}$", ErrorMessage = "手机号格式不正确!")]
        public string Mobile
        {
            get; set;
        }
        /// <summary>
        /// 自我描述
        /// </summary>
        public string Description
        {
            set; get;
        }
        /// <summary>
        /// 用户地址
        /// </summary>
        public string Address
        {
            get; set;
        }
        /// <summary>
        /// 用户毕业学校
        /// </summary>
        public string School
        {
            get; set;
        }
    }
}