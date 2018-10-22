using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [Table("Blog")]
    public class Blog:BaseEntity
    {
        /// <summary>
        /// 博客标题
        /// </summary>
        [StringLength(200)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [Column(TypeName = "text")]
        public string Content { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNumber { get; set; } = 0;

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentNumber { get; set; } = 0;

        /// <summary>
        /// 是否显示在首页
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsHome { get; set; } = "N";

        /// <summary>
        /// 是否显示在个人主页
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsMyHome { get; set; } = "Y";
        /// <summary>
        /// 是否是转发文章
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsForward { get; set; } = "N";

        /// <summary>
        /// 原链接
        /// </summary>
        [StringLength(255)]
        public string ForwardUrl { get; set; }
        /// <summary>
        /// 原博客发表时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? OldPublishTime { get; set; }

        // public int? UserId { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary>       
        [ForeignKey("BlogId")]
        public virtual ICollection<BlogBlogTag> BlogBlogTags { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary> 
        [ForeignKey("BlogId")]
        public virtual ICollection<BlogBlogType> BlogBlogTypes { get; set; }

        /// <summary>
        /// 博客评论
        /// </summary>      
        [ForeignKey("BlogId")]
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual UserInfo User { get; set; }
    }
}
