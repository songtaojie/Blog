using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [Table("User")]
    [Serializable]
    public class User:BaseModel,IEntity<int>
    {

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自增
        public int Id { get; set; }
        [NotMapped]
        public virtual string HexId
        {
            get { return Hx.Common.Helper.Helper.ToHex(Id); }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(50)]
        [Required]
        [Display(Name = "用户名")]
        public string UserName
        {
            get;set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [Display(Name = "密码")]
        [StringLength(40)]
        public string PassWord
        {
            set;
            get;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(100)]
        public string NickName { get; set; }
        /// <summary>
        /// 真实的名字
        /// </summary>
        [StringLength(100)]
        public string RealName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(20)]
        public string CardId
        {
            set; get;
        }
        /// <summary>
        /// 生日
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? Birthday
        {
            set; get;
        }

        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(8)]
        public string Gender
        {
            set; get;
        }
        /// <summary>
        /// 用户的QQ
        /// </summary>
        [StringLength(20)]
        public string QQ
        {
            get; set;
        }
        /// <summary>
        /// 用户微信号
        /// </summary>
        [StringLength(20)]
        public string WeChat
        {
            get;set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(80)]
        [Required]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        [Display(Name = "邮箱")]
        public string Email
        {
            set; get;
        }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(20)]
        public string Telephone
        {
            set; get;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(20)]
        public string Mobile
        {
            get; set;
        }
        /// <summary>
        /// 自我描述
        /// </summary>
        [StringLength(1000)]
        public string Description
        {
            set; get;
        }

        /// <summary>
        /// 第三方登录唯一标识
        /// </summary>
        [StringLength(80)]
        public string OpenId { get; set; }

        /// <summary>
		/// 是都锁定
		/// </summary>
        public bool IsLock { set; get; }
        /// <summary>
        /// 头像存储文件路径
        /// </summary>
        [StringLength(100)]
        public string AvatarUrl
        {
            get; set;
        }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin
        {
            get; set;
        }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate
        {
            get; set;
        }
        /// <summary>
        /// 用户注册时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? RegisterTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 用户注册时的ip
        /// </summary>
        public string RegisterIp
        {
            get;set;
        }
        /// <summary>
        /// 是否被删除,假删除，数据库中还有记录
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 用户地址
        /// </summary>
        [StringLength(100)]
        public string Address
        {
            get;set;
        }
        /// <summary>
        /// 用户毕业学校
        /// </summary>
        [StringLength(100)]
        public string School
        {
            get;set;
        }
        /// <summary>
        /// 使用MarkDown编辑器
        /// </summary>
        public virtual bool UseMdEdit { get; set; }
    }
}
