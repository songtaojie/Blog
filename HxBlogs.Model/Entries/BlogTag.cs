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
        [StringLength(40)]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
