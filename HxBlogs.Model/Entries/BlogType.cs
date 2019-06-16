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
    /// 博客的类型
    /// </summary>
    [Table("BlogType")]
    [Serializable]
    public class BlogType: BaseEntity
    {
        /// <summary>
        /// 名字
        /// </summary>
        [StringLength(40)]
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }
    }
}
