using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public abstract class BaseEntity : BaseModel, IEntity<long>
    {
        [Key]
        public virtual long Id { get; set; }

        [NotMapped]
        public virtual string HexId
        {
            get { return Hx.Common.Helper.Helper.ToHex(Id); }
        }
        #region 创建
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 这条记录属于哪个用户
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 用户的登录名称
        /// </summary>
        [StringLength(50)]
        public virtual string UserName { get; set; }
        #endregion

        #region 删除
        /// <summary>
        /// 是否被删除
        /// </summary>
        [NotMapped]
        public virtual bool IsDelete
        {
            get
            {
                return Hx.Common.Helper.Helper.IsYes(Delete);
            }
        }

        [StringLength(1)]
        [Column(TypeName = "char")]
        public virtual string Delete
        {
            get; set;
        } = "N";
        /// <summary>
        /// 删除人
        /// </summary>
        public virtual long? DeleteId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; }
        #endregion
    }
}
