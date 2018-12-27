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
        public override Blog BeforeInsert(Blog model)
        {
            model = base.BeforeInsert(model);
            if (Common.Helper.Helper.IsYes(model.IsPublish))
            {
                model.PublishDate = DateTime.Now;
            }
            return model;
        }
    }
}
