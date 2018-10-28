using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HxBlogs.IDAL;

namespace HxBlogs.BLL
{
    public partial class BlogService : BaseService<Blog>, IBlogService
    {
    }
}
