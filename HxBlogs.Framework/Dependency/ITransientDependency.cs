using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Framework.Dependency
{
    /// <summary>
    /// 每个依赖一个实例(即每次都重新实例),使用每个依赖的作用域, 
    /// 当你解析一个每个依赖一个实例的组件时, 你每次获得一个新的实例.
    /// </summary>
    public interface ITransientDependency
    {
    }
}
