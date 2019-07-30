using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_Index",
                "admin",
                new { controller ="home",  action = "Index"},
                new string[] { "HxBlogs.WebApp.Areas.Admin.Controllers" }
            );
            context.MapRoute(
               "Admin_Blog",
               "admin/b/{action}/{id}",
               new { controller="Blog", action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
                "Admin_Default",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}