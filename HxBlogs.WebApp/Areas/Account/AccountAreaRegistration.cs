using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute("HxBlogs_Register", "register", new
            {
                area = "Account",
                controller = "account",
                action = "register"
            });
            context.MapRoute("HxBlogs_Login", "login", new
            {
                area = "Account",
                controller = "account",
                action = "login"
            });
            context.MapRoute(
               "UC_Personal",
               "uc/p/{action}/{id}",
               new {controller= "Personal", action = "Profiles", id = UrlParameter.Optional }
           );
            context.MapRoute(
              "UC_Account",
              "uc/a/{action}/{id}",
              new { controller = "Account", action = "Login", id = UrlParameter.Optional }
          );
            context.MapRoute(
                "UC_default",
                "uc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}