using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    [Table("Attention")]
    public class Attention: BaseEntity
    {
        /// <summary>
        /// 关注的用户的id
        /// </summary>
        public long AttentionId
        {
            get;set;
        }
    }
}
