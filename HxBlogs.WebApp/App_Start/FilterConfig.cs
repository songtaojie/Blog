using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HxBlogs.WebApp.Filters.HandleErrorAttribute());
            filters.Add(new HxBlogs.WebApp.Filters.AuthFilterAttribute());
        }
    }
}
