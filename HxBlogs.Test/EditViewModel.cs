using HxBlogs.Framework.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Test
{
    public class EditViewModel:IAutoMapper<Model.Blog>
    {
        public string Title
        {
            get; set;
        }
        public string ContentHtml
        {
            get; set;
        }
        public int BlogID
        {
            get; set;
        }
        public int CatID
        {
            get; set;
        }
        public string PersonTop
        {
            get; set;
        }
        public string IsPrivate
        {
            get; set;
        }
        public string IsPublish
        {
            get; set;
        } = "Y";
    }
}
