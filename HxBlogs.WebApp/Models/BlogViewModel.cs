using Hx.Common.Helper;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HxBlogs.WebApp.Models
{
    public class BlogViewModel
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public string HexId
        {
            get
            {
                return Helper.ToHex(Id.ToString());
            }
        }
        public string ImgUrl { get; set; }
        public string ContentHtml { get; set; }
        public long UserId { get; set; }
        public UserInfo User { get; set; }
        public string UserName { get; set; }
        public DateTime? PublishDate { get; set; }
        public long ReadCount { get; set; }
        public long CmtCount { get; set; }
    }
}