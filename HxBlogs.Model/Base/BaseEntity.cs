using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public abstract class BaseEntity : BaseModel, IEntity<int>
    {
        [Key]
        public virtual int Id { get; set; }

        #region 创建
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人ID
        /// </summary>
        public virtual int? CreatorId { get; set; }
        /// <summary>
        /// 创建人名字
        /// </summary>
        [StringLength(50)]
        public virtual string CreatorName { get; set; }
        #endregion

        #region 删除
        /// <summary>
        /// 是否被删除
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public virtual string IsDeleted { get; set; } = "N";

        /// <summary>
        /// 删除人
        /// </summary>
        public virtual int? DeleteId { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        [StringLength(50)]
        public virtual string DeleteName { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? DeleteTime { get; set; }
        #endregion

        #region 修改
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 最后修改人ID
        /// </summary>
        public virtual int? LastModifiyId { get; set; }
        /// <summary>
        /// 最后修改人Name
        /// </summary>
        [StringLength(50)]
        public virtual string LastModifiyName { get; set; }
        #endregion

    }
}
