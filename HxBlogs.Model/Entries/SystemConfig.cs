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
    [Table("SystemConfig")]
    public class SystemConfig : BaseModel, IEntity<long>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// 是否允许注册
        /// </summary>
        public bool AllowRegister
        {
            get { return Helper.IsYes(Register); }
        }
        /// <summary>
        /// 是否可以注册，Y代表可以，N代表不可以
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string Register { get; set; } = "N";

    }
}
