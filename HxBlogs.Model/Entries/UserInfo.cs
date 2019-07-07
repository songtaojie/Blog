using Hx.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [Table("UserInfo")]
    [Serializable]
    public class UserInfo:BaseModel,IEntity<long>
    {

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自增
        public long Id { get; set; }

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
        /// 邮箱
        /// </summary>
        [StringLength(100)]
        [Required]
        [Display(Name = "邮箱")]
        public string Email
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
        [NotMapped]
        public bool IsLock
        {
            get { return Helper.IsYes(Lock); }
        }
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Lock { set; get; } = "N";
        /// <summary>
        /// 头像存储文件路径
        /// </summary>
        [StringLength(500)]
        public string AvatarUrl
        {
            get; set;
        }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        [NotMapped]
        public bool IsAdmin
        {
            get { return Helper.IsYes(Admin); }
        }
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Admin
        {
            get; set;
        } = "N";
        /// <summary>
        /// 是否激活
        /// </summary>
        [NotMapped]
        public bool IsActivate
        {
            get { return Helper.IsYes(Activate); }
        }
        /// <summary>
        /// 是否激活
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Activate
        {
            get; set;
        } = "N";
        /// <summary>
        /// 用户注册时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime RegisterTime { get; set; } = DateTime.Now;
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
        [NotMapped]
        public bool IsDeleted
        {
            get { return Helper.IsYes(Delete); }
        }
        /// <summary>
        /// 是否被删除,假删除，数据库中还有记录
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Delete { get; set; } = "N";

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? DeleteTime { get; set; }
       
        /// <summary>
        /// 使用MarkDown编辑器
        /// </summary>
        [NotMapped]
        public bool IsUseMdEdit
        {
            get { return Helper.IsYes(UseMdEdit); }
        }
        // <summary>
        /// 使用MarkDown编辑器
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string UseMdEdit { get; set; } = "N";
        /// <summary>
        /// 登录的ip
        /// </summary>
        [StringLength(100)]
        public string LoginIp { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; }
        /// <summary>
        /// 基础信息的ID
        /// </summary>
        public long BasicId
        {
            get;set;
        }
        /// <summary>
        /// 基础信息
        /// </summary>
        [ForeignKey("BasicId")]
        public virtual BasicInfo BasicInfo
        {
            get;set;
        }
        /// <summary>
        /// 职位信息的ID
        /// </summary>
        public long JobId
        {
            get;set;
        }
        /// <summary>
        /// 职位信息
        /// </summary>
        [ForeignKey("JobId")]
        public virtual JobInfo JobInfo
        {
            get; set;
        }
    }
}
