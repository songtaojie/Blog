using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Model
{
    /// <summary>
    /// 是否只允许管理员访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class AllowAdminAttribute:Attribute
    {
    }
}
