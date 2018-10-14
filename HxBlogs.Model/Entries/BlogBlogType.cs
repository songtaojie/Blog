using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    /// <summary>
    /// 博客和类型中间表
    /// </summary>
    [Table("BlogBlogType")]
    public class BlogBlogType:IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// 博客ID
        /// </summary>
        public int BlogId { get; set; }
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }
        [ForeignKey("TypeId")]
        public virtual BlogType BlogType { get; set; }
    }
}
