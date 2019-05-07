using Hx.Framework;
using HxBlogs.BLL;
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
        private IBlogTagService _tagService;
        public EditController(IBlogService blogService, IBlogTagService tagService)
        {
            _blogService = blogService;
            _tagService = tagService;
        }
        // GET: PostEdit
        public ActionResult PostEdit()
        {
            IBlogTypeService typeService = ContainerManager.Resolve<IBlogTypeService>();
            ICategoryService cateService = ContainerManager.Resolve<ICategoryService>();
            IEnumerable<Category> cateList = cateService.QueryEntities(c => c.IsDeleted == "N").OrderByDescending(c=>c.Order);
            IEnumerable<BlogType> typeList = typeService.QueryEntities(t => t.IsDeleted == "N").OrderByDescending(t => t.Order);
            List<BlogTag> tagList = _tagService.QueryEntities(t => t.UserId == UserContext.LoginUser.Id && t.IsDeleted == "N").ToList();
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
                TransactionManager.Excute(delegate
                {
                    Blog blogInfo = MapperManager.Map<Blog>(editInfo);
                    DbContextManager dbContext = new DbContextManager();
                    string[] imgList = WebHelper.GetHtmlImageUrlList(blogInfo.ContentHtml);
                    if (imgList.Length > 0) blogInfo.ImgUrl = imgList[1];
                    blogInfo.Content = HttpUtility.HtmlEncode(blogInfo.ContentHtml);
                    blogInfo = FillAddModel(blogInfo);
                    blogInfo = dbContext.Add(blogInfo);
                    //blogInfo = _blogService.Insert(blogInfo);
                    if (!string.IsNullOrEmpty(editInfo.PersonTags))
                    {
                        Dictionary<string, string> dicts = JsonConvert.DeserializeObject<Dictionary<string, string>>(editInfo.PersonTags);
                        foreach (KeyValuePair<string, string> item in dicts)
                        {
                            if (item.Key.Contains("newData") && !string.IsNullOrEmpty(item.Value))
                            {
                                string value = item.Value.Trim();
                                if (!_tagService.Exist(b => b.Name == value))
                                {
                                    BlogTag tag = new BlogTag()
                                    {
                                        Name = value,
                                    };
                                    if (UserContext.LoginUser != null)
                                    {
                                        tag.UserId = UserContext.LoginUser.Id;
                                    }
                                    FillAddModel(tag);
                                    tag = dbContext.Add(tag);

                                    BlogBlogTag blogTag = new BlogBlogTag()
                                    {
                                        BlogId = blogInfo.Id,
                                        TagId = tag.Id
                                    };
                                    blogTag = dbContext.Add(blogTag);
                                }
                            }
                            else
                            {
                                BlogBlogTag blogTag = new BlogBlogTag()
                                {
                                    BlogId = blogInfo.Id,
                                    TagId = Convert.ToInt32(item.Key)
                                };
                                blogTag = dbContext.Add(blogTag);
                            }
                        }
                    }
                    dbContext.SaveChages();
                });
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