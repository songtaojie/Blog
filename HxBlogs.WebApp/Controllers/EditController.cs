using HxBlogs.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class EditController : BaseController
    {
        private ICategoryService _cateService;
        public EditController(ICategoryService cateService)
        {
            this._cateService = cateService;
        }
        // GET: PostEdit
        public ActionResult PostEdit()
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
        public ActionResult Save(Models.EditViewModel editInfo)
        {
            ReturnResult result = new ReturnResult();
            Common.Json.JsonHelper.ToJsonInclude(result, nameof(result.IsSuccess), nameof(result.Message));
            
            if (ModelState.IsValid)
            {
                var coll = ModelState.GetEnumerator();
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelstate = ModelState[key];
                    if (modelstate.Errors.Any())
                    {
                        string errorMessage = modelstate.Errors.FirstOrDefault().ErrorMessage;
                    }
                }
            }
            return Json(new { success=true},JsonRequestBehavior.AllowGet);
        }
    }
}