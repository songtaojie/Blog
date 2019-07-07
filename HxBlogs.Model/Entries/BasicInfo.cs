using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    /// <summary>
    /// 用户的基本信息
    /// </summary>
    [Table("BasicInfo")]
    public class BasicInfo : BaseEntity
    {
        /// <summary>
        /// 真实的名字
        /// </summary>
        [StringLength(100)]
        public string RealName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(40)]
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
        [StringLength(40)]
        public string QQ
        {
            get; set;
        }
        /// <summary>
        /// 用户微信号
        /// </summary>
        [StringLength(40)]
        public string WeChat
        {
            get; set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(40)]
        public string Telephone
        {
            set; get;
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(40)]
        public string Mobile
        {
            get; set;
        }
        /// <summary>
        /// 自我描述
        /// </summary>
        [StringLength(2000)]
        public string Description
        {
            set; get;
        }

        /// <summary>
        /// 用户地址
        /// </summary>
        [StringLength(200)]
        public string Address
        {
            get; set;
        }
        /// <summary>
        /// 用户毕业学校
        /// </summary>
        [StringLength(200)]
        public string School
        {
            get; set;
        }
    }
}
