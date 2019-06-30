using Hx.Framework.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class UserViewModel: IAutoMapper<Model.User>
    {
        public long Id { get; set; }
        public virtual string HexId
        {
            get { return Hx.Common.Helper.Helper.ToHex(Id); }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get; set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord
        {
            set;
            get;
        }
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
        public string Gender
        {
            set; get;
        }
        /// <summary>
        /// 用户的QQ
        /// </summary>
        public string QQ
        {
            get; set;
        }
        /// <summary>
        /// 用户微信号
        /// </summary>
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
        /// 头像存储文件路径
        /// </summary>
        public string AvatarUrl
        {
            get; set;
        }
        /// <summary>
        /// 用户注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 用户注册时的ip
        /// </summary>
        public string RegisterIp
        {
            get; set;
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