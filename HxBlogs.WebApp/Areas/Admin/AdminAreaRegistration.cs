using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute("HxBlogs_Register", "Register", new
            {
                area = "Admin",
                controller = "Account",
                action = "Register"
            });
            context.MapRoute("HxBlogs_Login", "Login", new
            {
                area = "Admin",
                controller = "Account",
                action = "Index"
            });
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}