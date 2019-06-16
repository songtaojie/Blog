using HxBlogs.Model;
using HxBlogs.Model.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.Test
{
    public class DbContextTest
    {
        public static void QuereyTest()
        {
            DbContext db = DbFactory.GetDbContext();
            var result = db.Set<Blog>()
                .AsNoTracking()
                .Where(b => b.Delete == "N")
                .Select(b => new BlogViewModel
            {
                Id = b.Id,
                Title = b.Title
            });
            foreach (BlogViewModel item in result)
            {
                Console.WriteLine(item.Title);
            }
        }
    }

    public class BlogViewModel
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public string ContentHtml { get; set; }

        public string UserName { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? ReadCount { get; set; }
        public int? CmtCount { get; set; }
    }
}
