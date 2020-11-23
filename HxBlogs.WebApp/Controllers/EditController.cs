using Hx.Common.Helper;
using Hx.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.Transactions;
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
            UserInfo user = UserContext.LoginUser;
            if (string.IsNullOrEmpty(hexId))
            {
                string view = "ckedit";
                if (user != null && user.IsUseMdEdit)
                {
                    view = "mdedit";
                }
                return RedirectToAction(view);
            }
            else
            {
                long blogId = Convert.ToInt64(Helper.FromHex(hexId));
                Blog blog = this._blogService.GetEntityBySql("id="+ blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                string view = "ckedit";
                if (blog.IsMarkDown)
                {
                    view = "mdedit";
                }
                return Redirect(string.Format("/edit/{0}/{1}",view,hexId));
            }
            
        }
        [Route("ckedit/{hexId?}")]
        public ActionResult CkEdit(string hexId)
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
            EditViewModel vm = new EditViewModel() { MarkDown = isMd?"Y":"N"};
            if (string.IsNullOrEmpty(hexId))
            {
                if(isMd)vm.Content = FileHelper.GetString(Server.MapPath("~/Plugins/editormd/template.md"));
            }
            else
            {
                long blogId = Convert.ToInt64(Helper.FromHex(hexId));
                Blog blog = this._blogService.GetEntityByID(blogId);
                if (blog == null) throw new NotFoundException("找不到当前文章!");
                vm = MapperManager.Map<EditViewModel>(blog);
                vm.PersonTags = blog.BlogTags;
                vm.HexId = hexId;
                if (!string.IsNullOrEmpty(blog.BlogTags))
                {
                    Dictionary<long, string> tagDic = new Dictionary<long, string>();
                    string[] tagArr = blog.BlogTags.Split(',');
                    foreach (string tagId in tagArr)
                    {
                        BlogTag blogTag = _tagService.GetEntityByID(Convert.ToInt32(tagId));
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
            IEnumerable<Category> categories = cateService.GetEntities(c => true).
                OrderByDescending(c => c.Order);
            IEnumerable<BlogType> types = typeService.GetEntities(t => true).
                OrderByDescending(t => t.Order);
            IEnumerable<BlogTag> tags = _tagService.GetEntities(t => t.UserId == UserContext.LoginUser.Id);
            ViewBag.Categories = categories;
            ViewBag.Types = types;
            ViewBag.Tags = tags;
            return View(vm);
        }
        #region 博客的保存编辑
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
                        blogInfo.Id = Convert.ToInt64(Helper.FromHex(editInfo.HexId));
                    }
                    DbContextManager dbContext = new DbContextManager();
                    string[] imgList = WebHelper.GetHtmlImageUrlList(blogInfo.ContentHtml);
                    if (imgList.Length > 0)
                    {
                        blogInfo.ImgUrl = string.Join(",", imgList);
                    }
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
                                    BlogTag tag = _tagService.GetEntity(b => b.Name == value);
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
                            "Title", "ContentHtml","Content", "TypeId", "CatId", "PersonTop", "Private",
                            "Publish", "CanCmt", "MarkDown", "BlogTags", "PublishDate");
                    }
                    else
                    {
                        blogInfo = FillAddModel(blogInfo);
                        blogInfo = _blogService.Insert(blogInfo);
                    }
                    result.Resultdata = "/article/"+ blogInfo .UserName + "/"+blogInfo.HexId;
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
        #endregion
    }
}