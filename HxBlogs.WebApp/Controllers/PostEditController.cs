using HxBlogs.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class PostEditController : BaseController
    {
        private ICategoryService _cateService;
        public PostEditController(ICategoryService cateService)
        {
            this._cateService = cateService;
        }
        // GET: PostEdit
        public ActionResult Edit()
        {
            IBlogTypeService typeService = ContainerManager.Resolve<IBlogTypeService>();
            IBlogTagService tagService = ContainerManager.Resolve<IBlogTagService>();
            IEnumerable<Category> cateList =  this._cateService.QueryEntities(c => c.IsDeleted == "N").OrderByDescending(c=>c.Order);
            IEnumerable<BlogType> typeList = typeService.QueryEntities(t => t.IsDeleted == "N").OrderByDescending(t => t.Order);
            IEnumerable<BlogTag> tagList = tagService.QueryEntities(t => t.UserId == 0);
            ViewBag.CategoryList = cateList;
            ViewBag.BlogTypeList = typeList;
            ViewBag.BlogTagList = tagList; 
            return View();
        }
    }
}