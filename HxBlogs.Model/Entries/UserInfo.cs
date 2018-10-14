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
    public class UserInfo:BaseModel,IEntity<int>
    {

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自增
        public int Id { get; set; }
        ///// <summary>
        ///// 用户ID
        ///// </summary>
        //[StringLength(40)]
        //[Required]
        //public string UserId
        //{
        //    get;set;
        //}
        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string UserName
        {
            get;set;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(100)]
        public string NickName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(40)]
        public string PassWord
        {
            set;
            get;
        }
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
        /// 邮箱
        /// </summary>
        [StringLength(80)]
        public string Email
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
        /// 电话
        /// </summary>
        [StringLength(20)]
        public string Telephone
        {
            set; get;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "text")]
        public string Remarks
        {
            set; get;
        }

        /// <summary>
        /// 第三方登录唯一标识
        /// </summary>
        [StringLength(80)]
        public string OpenId { get; set; }

        /// <summary>
		/// 
		/// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string CanLogin { set; get; } = "Y";
        /// <summary>
        /// 头像文件名字
        /// </summary>
        [StringLength(100)]
        public string AvatarName
        {
            get; set;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(20)]
        public string Mobile
        {
            get; set;
        }
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsRoot
        {
            get; set;
        } = "N";
    }
}
