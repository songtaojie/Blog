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
    [RoutePrefix("edit")]
    [Route("{action}")]
    public class EditController : BaseController
    {
        private IBlogService _blogService;
        private IBlogTagService _tagService;
        public EditController(IBlogService blogService, IBlogTagService tagService)
        {
            _blogService = blogService;
            _tagService = tagService;
        }
        [Route("postedit/{hexId?}")]
        public ActionResult PostEdit(string hexId)
        {
            User user = UserContext.LoginUser;
            if (string.IsNullOrEmpty(hexId))
            {
                string view = "richedit";
                if (user != null && user.UseMdEdit)
                {
                    view = "mdedit";
                }
                return RedirectToAction(view);
            }
            else
            {
                int blogId = Convert.ToInt32(Helper.FromHex(hexId));
                Blog blog = this._blogService.QueryEntityByID(blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                string view = "richedit";
                if (blog.IsMarkDown)
                {
                    view = "mdedit";
                }
                return Redirect(string.Format("/edit/{0}/{1}",view,hexId));
            }
            
        }
        [Route("richedit/{hexId?}")]
        public ActionResult RichEdit(string hexId)
        {
            return Edit(hexId, false);
        }
        [Route("mdedit/{hexId?}")]
        public ActionResult MdEdit(string hexId)
        {
            return Edit(hexId,true);
        }
        [NonAction]
        private ActionResult Edit(string hexId,bool isMd)
        {
            EditViewModel vm = new EditViewModel() { IsMarkDown = isMd};
            if (string.IsNullOrEmpty(hexId))
            {
                if(isMd)vm.Content = FileHelper.GetString(Server.MapPath("~/Plugins/editormd/template.md"));
            }
            else
            {
                int blogId = Convert.ToInt32(Helper.FromHex(hexId));
                Blog blog = this._blogService.QueryEntityByID(blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                vm = MapperManager.Map<EditViewModel>(blog);
                vm.PersonTags = blog.BlogTags;
                vm.HexId = hexId;
                if (!string.IsNullOrEmpty(blog.BlogTags))
                {
                    Dictionary<int, string> tagDic = new Dictionary<int, string>();
                    string[] tagArr = blog.BlogTags.Split(',');
                    foreach (string tagId in tagArr)
                    {
                        BlogTag blogTag = _tagService.QueryEntityByID(Convert.ToInt32(tagId));
                        if (blogTag != null)
                        {
                            tagDic.Add(blogTag.Id, blogTag.Name);
                        }
                    }
                    ViewBag.PersonTags = tagDic;
                }
            }
            if (string.IsNullOrEmpty(vm.PersonTags)) vm.PersonTags = "";
            IBlogTypeService typeService = ContainerManager.Resolve<IBlogTypeService>();
            ICategoryService cateService = ContainerManager.Resolve<ICategoryService>();
            IEnumerable<Category> cateList = cateService.QueryEntities(c => true).OrderByDescending(c => c.Order);
            IEnumerable<BlogType> types = typeService.QueryEntities(t => true).OrderByDescending(t => t.Order);
            IEnumerable<BlogTag> tags = _tagService.QueryEntities(t => t.UserId == UserContext.LoginUser.Id);
            ViewBag.CategoryList = cateList;
            ViewBag.Types = types;
            ViewBag.Tags = tags;
            return View(vm);
        }
        [HttpPost]
        public ActionResult Save(Models.EditViewModel editInfo)
        {
            AjaxResult result = new AjaxResult();
            if (ModelState.IsValid)
            {
                bool isEdit = false;
                TransactionManager.Excute(delegate
                {
                    Blog blogInfo = MapperManager.Map<Blog>(editInfo);
                    if (!string.IsNullOrEmpty(editInfo.HexId))
                    {
                        isEdit = true;
                        blogInfo.Id = Convert.ToInt32(Helper.FromHex(editInfo.HexId));
                    }
                    DbContextManager dbContext = new DbContextManager();
                    string[] imgList = WebHelper.GetHtmlImageUrlList(blogInfo.ContentHtml);
                    if (imgList.Length > 0) blogInfo.ImgUrl = imgList[1];
                    if (!blogInfo.IsMarkDown)
                    {
                        blogInfo.Content = HttpUtility.HtmlEncode(blogInfo.ContentHtml);
                    }
                    if (blogInfo.IsPublish)
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
                    if (isEdit)
                    {
                        _blogService.UpdateEntityFields(blogInfo,
                            "Title", "ContentHtml","Content", "TypeId", "CatId", "PersonTop", "IsPrivate",
                            "IsPublish", "CanCmt", "IsMarkDown", "BlogTags", "PublishDate");
                    }
                    else
                    {
                        blogInfo = FillAddModel(blogInfo);
                        _blogService.Insert(blogInfo);
                    }
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