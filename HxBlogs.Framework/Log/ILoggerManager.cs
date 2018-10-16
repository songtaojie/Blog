using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Framework.Log
{
    public interface ILoggerManager
    {
        ILogger CreateLogger<T>(string group);
    }
}
