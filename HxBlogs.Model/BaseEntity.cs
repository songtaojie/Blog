using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public class BaseEntity : BaseModel, IEntity<int>
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
        [StringLength(40)]
        public virtual int CreatorId { get; set; }
        /// <summary>
        /// 创建人名字
        /// </summary>
        [StringLength(50)]
        public virtual int CreatorName { get; set; }
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
        [StringLength(40)]
        public virtual string DeleteId { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        [StringLength(50)]
        public virtual int? DeleteName { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? DeleteTime { get; set; } = DateTime.Now;
        #endregion

        #region 修改
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public virtual DateTime? LastModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改人ID
        /// </summary>
        [StringLength(40)]
        public virtual int? LastModifiyId { get; set; }
        /// <summary>
        /// 最后修改人Name
        /// </summary>
        [StringLength(50)]
        public virtual string LastModifiyName { get; set; }
        #endregion

    }
}
