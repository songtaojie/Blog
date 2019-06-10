using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    public interface IParameter
    {
        /// <summary>
        /// 根据查询名字创建查询参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        DbParameter Create(string paramName);

        /// <summary>
        /// 查询参数的值
        /// </summary>
        object ParamValue { get; set; }
    }
}
