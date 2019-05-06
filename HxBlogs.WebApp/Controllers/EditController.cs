using Hx.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using Newtonsoft.Json;
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
        private IBlogService _blogService;
        public EditController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        // GET: PostEdit
        public ActionResult PostEdit()
        {
            IBlogTypeService typeService = ContainerManager.Resolve<IBlogTypeService>();
            IBlogTagService tagService = ContainerManager.Resolve<IBlogTagService>();
            ICategoryService cateService = ContainerManager.Resolve<ICategoryService>();
            IEnumerable<Category> cateList = cateService.QueryEntities(c => c.IsDeleted == "N").OrderByDescending(c=>c.Order);
            IEnumerable<BlogType> typeList = typeService.QueryEntities(t => t.IsDeleted == "N").OrderByDescending(t => t.Order);
            List<BlogTag> tagList = tagService.QueryEntities(t => t.UserId == UserContext.LoginUser.Id).ToList();
            ViewBag.CategoryList = cateList;
            ViewBag.BlogTypeList = typeList;
            ViewBag.BlogTagList = tagList; 
            return View();
        }
        public ActionResult Save(Models.EditViewModel editInfo)
        {
            ReturnResult result = new ReturnResult {IsSuccess = true };
            if (ModelState.IsValid)
            {
                Blog blogInfo = MapperManager.Map<Blog>(editInfo);
                string[] imgList = WebHelper.GetHtmlImageUrlList(blogInfo.ContentHtml);
                if (imgList.Length > 0) blogInfo.ImgUrl = imgList[1];
                blogInfo.Content = HttpUtility.HtmlEncode(blogInfo.ContentHtml);
                blogInfo = FillAddModel(blogInfo);
                if (!string.IsNullOrEmpty(editInfo.PersonTags))
                {
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(editInfo.PersonTags);
                    //foreach()
                }
                //_blogService.Insert(blogInfo);
            }
            else
            {
                result.IsSuccess = false;
                foreach (var key in ModelState.Keys)
                {
                    var modelstate = ModelState[key];
                    if (modelstate.Errors.Any())
                    {
                        result.Message = modelstate.Errors.FirstOrDefault().ErrorMessage;
                        break;
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}