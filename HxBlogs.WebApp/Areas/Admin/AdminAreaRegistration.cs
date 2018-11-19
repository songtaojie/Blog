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
                area = "admin",
                controller = "account",
                action = "register"
            });
            context.MapRoute("HxBlogs_Login", "Login", new
            {
                area = "admin",
                controller = "account",
                action = "index"
            });
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}