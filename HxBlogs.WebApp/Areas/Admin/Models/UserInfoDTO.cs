using Hx.Framework.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class UserInfoDTO: IAutoMapper<Model.UserInfo>
    {
        public long Id { get; set; }
        public virtual string HexId
        {
            get { return Hx.Common.Helper.Helper.ToHex(Id); }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        [Display(Name = "用户名")]
        [StringLength(50)]
        [Required(ErrorMessage = "用户名是必填项")]
        [RegularExpression("^([a-zA-Z]|[\u4E00-\u9FA5])([a-zA-Z0-9]|[\u4E00-\u9FA5]|[_]){4,31}$",
            ErrorMessage = "用户名由字母、数字、下划线和中文组成，以中文或字母开头，且长度为4~30")]
        public string UserName
        {
            get; set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [RegularExpression("^.*(?=.{6,16})(?=.*\\d)(?=.*[A-Z]{1,})(?=.*[a-z]{1,})(?=.*[.!@#$%^&*]).*$",
            ErrorMessage = "密码必须包含数字、大小写字母、和一个特殊符号,且长度为6~16")]
        public string PassWord
        {
            set;
            get;
        }

        [Compare("PassWord", ErrorMessage = "两次所输密码不一样")]
        public string PwdConfirm
        {
            get; set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(80)]
        [Required(ErrorMessage = "邮箱是必填项")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email
        {
            set; get;
        }
    }
}