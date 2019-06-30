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
            context.MapRoute("HxBlogs_Register", "register", new
            {
                area = "admin",
                controller = "account",
                action = "register"
            });
            context.MapRoute("HxBlogs_Login", "login", new
            {
                area = "admin",
                controller = "account",
                action = "login"
            });
            context.MapRoute(
                "Uc_Route",
                "admin/uc/{action}/{*extend}",
                new { controller = "UserCenter", action = "Profiles", extend = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Default",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}