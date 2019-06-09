using Hx.Framework.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class EditViewModel: IAutoMapper<Model.Blog>
    {
        public string HexId
        {
            get;set;
        }
        [Required(ErrorMessage = "标题不能为空!")]
        [MaxLength(100,ErrorMessage = "请控制在100字以内")]
        public string Title
        {
            get;set;
        }
        [Required(ErrorMessage ="博客内容不能为空!")]
        [System.Web.Mvc.AllowHtml]
        public string ContentHtml
        {
            get;set;
        }
        [Required(ErrorMessage = "博客内容不能为空!")]
        [System.Web.Mvc.AllowHtml]
        public string Content
        {
            get; set;
        }
        [Required(ErrorMessage = "系统分类不能为空!")]
        public int TypeId
        {
            get;set;
        }
        [Required(ErrorMessage = "文章类型不能为空!")]
        public int CatId
        {
            get;set;
        }
        public bool PersonTop
        {
            get; set;
        }
        public bool IsPrivate
        {
            get; set;
        }
        public bool IsPublish
        {
            get; set;
        } = true;
        public bool CanCmt
        {
            get; set;
        } = true;
        public string PersonTags
        {
            get;set;
        }
        public bool IsMarkDown
        {
            get; set;
        }
    }
}