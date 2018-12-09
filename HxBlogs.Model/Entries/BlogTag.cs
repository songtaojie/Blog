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
    /// 博客标签
    /// </summary>
    [Table("BlogTag")]
    [Serializable]
    public class BlogTag:BaseEntity
    {
        /// <summary>
        /// 标签名字
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "text")]
        public string Remark { get; set; }

        /// <summary>
        /// 这个标签属于哪个用户，表示那个用户的ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 属于这个用户的标签
        /// </summary>
        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        /// <summary>
        /// 关联的博客
        /// </summary>
        [ForeignKey("TagId")]
        public virtual ICollection<BlogBlogTag> BlogBlogTags { get; set; }
    }
}
