using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class RegisterViewModel
    {
        [StringLength(50)]
        [MinLength(2,ErrorMessage ="用户名最少是两位数据")]
        [Required(ErrorMessage ="用户名是必填项")]
        [Display(Name = "用户名")]
        [RegularExpression("^[\u4e00-\u9fa5\\w]{1,30}$",ErrorMessage = "用户名由中文,数字,字母和下划线组成")]
        [System.Web.Mvc.Remote("CheckUserName","Account","Admin",HttpMethod ="Post")]
        public string UserName
        {
            get; set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage ="密码是必填项")]
        [Display(Name = "密码")]
        [StringLength(40)]
        public string PassWord
        {
            set;
            get;
        }

        [Required(ErrorMessage = "确认密码是必填项")]
        [StringLength(40)]
        [Display(Name = "确认密码")]
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
        [Display(Name = "邮箱")]
        [System.Web.Mvc.Remote("CheckEmail", "Account", "Admin", HttpMethod = "Post")]
        public string Email
        {
            set; get;
        }
    }
}