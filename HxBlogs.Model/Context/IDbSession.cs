using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model.Context
{
    /// <summary>
    /// 用来处理DbContext的一些操作，如保存更改等
    /// </summary>
    public interface IDbSession
    {
        /// <summary>
        /// 获取操作的上下文
        /// </summary>
        DbContext DbContext { get; }
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        bool SaveChange();
    }
}
