using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [Table("ReplyComment")]
    public class ReplyComment
    {
        /// <summary>
        /// 回复的评论id
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// 回复给用户
        /// </summary>      
        public int? ReplyToUserId { get; set; }

        /// <summary>
        /// 回复给用户的名字（为了兼容迁移博客）
        /// </summary>
        [StringLength(50)]
        public string ReplyToUserName { get; set; }

        /// <summary>
        /// 备用（以后可能显示地址）
        /// </summary>
        [StringLength(40)]
        public string IPAddress { get; set; }


        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        /// <summary>
        /// 回复给的用户
        /// </summary>
        [ForeignKey("ReplyToUserId")]
        public virtual UserInfo ReplyToUser { get; set; }
    }
}
