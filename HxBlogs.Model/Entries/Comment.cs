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
    /// 评论
    /// </summary>
    [Table("Comment")]
    [Serializable]
    public class Comment:BaseEntity
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        [StringLength(1000)]
        public string Content { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public bool Top { get; set; }

        /// <summary>
        /// 记录IP地址（用于天机ip显示地区）
        /// </summary>
        [StringLength(40)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 评论的博客
        /// </summary>
        public int BlogId { get; set; }

        /// <summary>
        /// 博客
        /// </summary>
        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }
    }
}
