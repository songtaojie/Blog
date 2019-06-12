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
        public int Id { get; set; }
        public string HexId
        {
            get
            {
                return Helper.ToHex(Id.ToString());
            }
        }
        public string ContentHtml { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? ReadCount { get; set; }
        public int? CmtCount { get; set; }
    }
}