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
    /// 博客和标签中间表
    /// </summary>
    [Table("BlogBlogTag")]
    public class BlogBlogTag : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 博客的ID
        /// </summary>
        public int BlogId { get; set; }
        /// <summary>
        /// 标签的ID
        /// </summary>
        public int TagId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }
        [ForeignKey("TagId")]
        public virtual BlogTag BlogTag { get; set; }
        
    }
}
