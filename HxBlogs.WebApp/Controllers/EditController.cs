using Hx.Common.Helper;
using Hx.Framework;
using HxBlogs.BLL;
using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.WebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [Route("edit/postedit/{hexId?}")]
        public ActionResult PostEdit(string hexId)
        {
            User user = UserContext.LoginUser;
            if (string.IsNullOrEmpty(hexId))
            {
                if (user != null && Helper.IsYes(user.UseMdEdit))
                {
                    return RedirectToAction("mdedit");
                }
                else
                {

                    return RedirectToAction("richedit");
                }
            }
            else
            {
                int blogId = Convert.ToInt32(Helper.FromHex(hexId));
                Blog blog = this._blogService.QueryEntityByID(blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                if (Helper.IsYes(blog.IsMarkDown))
                {
                    return RedirectToAction("mdedit", new { hexId });
                }
                else
                {
                    return RedirectToAction("richedit", new { hexId });
                }
            }
            
        }
        // GET: PostEdit
        public ActionResult RichEdit(string hexId)
        {
            EditViewModel vm = new EditViewModel();
            if (!string.IsNullOrEmpty(hexId))
            {
                int blogId = Convert.ToInt32(Helper.FromHex(hexId));
                Blog blog = this._blogService.QueryEntityByID(blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                vm = MapperManager.Map<EditViewModel>(blog);
            }
            IBlogTypeService typeService = ContainerManager.Resolve<IBlogTypeService>();
            ICategoryService cateService = ContainerManager.Resolve<ICategoryService>();
            IEnumerable<Category> cateList = cateService.QueryEntities(c =>true).OrderByDescending(c=>c.Order);
            IEnumerable<BlogType> typeList = typeService.QueryEntities(t =>true).OrderByDescending(t => t.Order);
            List<BlogTag> tagList = _tagService.QueryEntities(t => t.UserId == UserContext.LoginUser.Id).ToList();
            ViewBag.CategoryList = cateList;
            ViewBag.BlogTypeList = typeList;
            ViewBag.BlogTagList = tagList;
            return View(vm);
        }
        public ActionResult MdEdit(string hexId)
        {
            EditViewModel vm = new EditViewModel();
            if (string.IsNullOrEmpty(hexId))
            {
                vm.Content = FileHelper.GetString(Server.MapPath("~/Plugins/editormd/template.md"));
            }
            else
            {
                int blogId = Convert.ToInt32(Helper.FromHex(hexId));
                Blog blog = this._blogService.QueryEntityByID(blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                vm = MapperManager.Map<EditViewModel>(blog);
            }
            IBlogTypeService typeService = ContainerManager.Resolve<IBlogTypeService>();
            ICategoryService cateService = ContainerManager.Resolve<ICategoryService>();
            IEnumerable<Category> cateList = cateService.QueryEntities(c => true).OrderByDescending(c => c.Order);
            IEnumerable<BlogType> typeList = typeService.QueryEntities(t => true).OrderByDescending(t => t.Order);
            List<BlogTag> tagList = _tagService.QueryEntities(t => t.UserId == UserContext.LoginUser.Id).ToList();
            ViewBag.CategoryList = cateList;
            ViewBag.BlogTypeList = typeList;
            ViewBag.BlogTagList = tagList;
            return View(vm);
        }
        [HttpPost]
        public ActionResult Save(Models.EditViewModel editInfo)
        {
            AjaxResult result = new AjaxResult();
            if (ModelState.IsValid)
            {
                TransactionManager.Excute(delegate
                {
                    Blog blogInfo = MapperManager.Map<Blog>(editInfo);
                    DbContextManager dbContext = new DbContextManager();
                    string[] imgList = WebHelper.GetHtmlImageUrlList(blogInfo.ContentHtml);
                    if (imgList.Length > 0) blogInfo.ImgUrl = imgList[1];
                    if (!Helper.IsYes(blogInfo.IsMarkDown))
                    {
                        blogInfo.Content = HttpUtility.HtmlEncode(blogInfo.ContentHtml);
                    }
                    blogInfo = FillAddModel(blogInfo);
                    if (Helper.IsYes(blogInfo.IsPublish))
                    {
                        blogInfo.PublishDate = DateTime.Now;
                    }
                    List<string> tagList = new List<string>();
                    if (!string.IsNullOrEmpty(editInfo.PersonTags))
                    {
                        int fakeId = 0;
                        Dictionary<string, string> dicts = JsonConvert.DeserializeObject<Dictionary<string, string>>(editInfo.PersonTags);
                        foreach (KeyValuePair<string, string> item in dicts)
                        {
                            if (!string.IsNullOrEmpty(item.Value))
                            {
                                if (item.Key.Contains("newData"))
                                {
                                    string value = item.Value.Trim();
                                    BlogTag tag = _tagService.QueryEntity(b => b.Name == value);
                                    if (tag == null)
                                    {
                                        tag = new BlogTag()
                                        {
                                            Id = fakeId++,
                                            Name = value,
                                        };
                                        if (UserContext.LoginUser != null)
                                        {
                                            tag.UserId = UserContext.LoginUser.Id;
                                        }
                                        FillAddModel(tag);
                                        tag = _tagService.Insert(tag);
                                        // tag = dbContext.Add(tag);
                                    }
                                    tagList.Add(tag.Id.ToString());
                                }
                                else if(!string.IsNullOrEmpty(item.Key))
                                {
                                    tagList.Add(item.Key);
                                }
                            }
                        }
                    }
                    blogInfo.BlogTags = string.Join(",", tagList);
                    _blogService.Insert(blogInfo);
                    //blogInfo = dbContext.Add(blogInfo);
                    // dbContext.SaveChages();
                });
            }
            else
            {
                result.Success = false;
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