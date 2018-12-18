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
    [Serializable]
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
        /// 内容，html格式
        /// </summary>
        [Column(TypeName = "text")]
        public string ContentHtml { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int? ReadCount { get; set; } = 0;

        /// <summary>
        /// 评论数量
        /// </summary>
        public int? CommentCount { get; set; } = 0;
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsPrivate { get; set; } = "N";
        /// <summary>
        /// 是否是转发文章
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsForward { get; set; } = "N";
        /// <summary>
        /// 是否发布，Y代表发布，N代表不发布即是草稿
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsPublish { get; set; } = "N";

        /// <summary>
        /// 发布日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 置顶 Y权值加10年
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsTop { get; set; }
        /// <summary>
        /// 精华 Y权值加10天
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string IsEssence { get; set; }
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
        public int TypeId { get; set; }
        ///// <summary>
        ///// 博客类型，是转发，原创，还是翻译等
        ///// </summary> 
        [ForeignKey("TypeId")]
        public virtual BlogType BlogType { get; set; }

        public int CatId { get; set; }
        ///// <summary>
        ///// 博客类型，是转发，原创，还是翻译等
        ///// </summary> 
        [ForeignKey("CatId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// 博客评论
        /// </summary>      
        [ForeignKey("BlogId")]
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 允许评论
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string CanCmt { get; set; } = "Y";
        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual UserInfo User { get; set; }

        /// <summary>
        /// 博客被推荐的次数
        /// </summary>
        public int? LikeCount { get; set; }

        /// <summary>
        /// 被收藏次数
        /// </summary>
        public int? FavCount { get; set; }
        ///// <summary>
        ///// 被评论次数
        ///// </summary>
        //public int? CmtCount { get; set; }
        /// <summary>
        /// 个人置顶 标识该文档是否置顶,置顶的文章在个人主页中排序靠前
        /// </summary>
        [StringLength(1)]
        [Column(TypeName = "char")]
        public string PersonTop { get; set; }
        ///// <summary>
        ///// 浏览数
        ///// </summary>
        //public int? ViewCount { get; set; }

        /// <summary>
        /// 主题中的第一张图的地址
        /// </summary>
        [StringLength(255)]
        public string ImgUrl { get; set; }
        /// <summary>
        /// 主题中第一张图的名字
        /// </summary>
        [StringLength(100)]
        public string ImgName { get; set; }

        /// <summary>
        /// 发改主题时的坐标
        /// </summary>
        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(50)]
        public string City { get; set; }
        /// <summary>
        /// 热门程度
        /// </summary>
        [Column]
        [DecimalPrecision]
        public decimal? OrderFactor { get; set; }
    }
}
