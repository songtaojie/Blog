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
        [Required(ErrorMessage = "标题不能为空!")]
        [MaxLength(100,ErrorMessage = "请控制在100字以内")]
        public string Title
        {
            get;set;
        }
        [Required(ErrorMessage ="内容不能为空!")]
        [System.Web.Mvc.AllowHtml]
        public string ContentHtml
        {
            get;set;
        }
        [Required(ErrorMessage = "系统分类不能为空!")]
        public int BlogID
        {
            get;set;
        }
        [Required(ErrorMessage = "文章类型不能为空!")]
        public int CatID
        {
            get;set;
        }
        public string PersonTop
        {
            get;set;
        }
        public string IsPrivate
        {
            get;set;
        }
        public string IsPublish
        {
            get; set;
        } = "Y";
        public string PersonTags
        {
            get;set;
        }
    }
}