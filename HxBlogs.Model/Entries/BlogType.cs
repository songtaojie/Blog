using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    /// <summary>
    /// 博客的类型
    /// </summary>
    [Table("BlogType")]
    public class BlogType: BaseEntity
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 属于这个用户的类型
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual UserInfo User { get; set; }

        /// <summary>
        /// 关联的博客
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual ICollection<BlogBlogType> BlogBlogTypes { get; set; }
    }
}
