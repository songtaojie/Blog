using System.Web;
using System.Web.Optimization;

namespace HxBlogs.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region js文件
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/app/jquery.extendsion.js"));
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/local/messages_zh.js"));
            bundles.Add(new ScriptBundle("~/bundles/scroll").Include(
                        "~/Scripts/app/scrollReveal.js"));
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/Scripts/plugin/jquery.blockUI.js",
                        "~/Content/Plugin/alertifyjs/alertify.js",
                       "~/Scripts/app/hx.loading.js",
                       "~/Scripts/app/hx.core.js"));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            #endregion

            #region 样式文件
            bundles.Add(new StyleBundle("~/content/style").Include(
               "~/Content/bootstrap.css",
               "~/font/font-awesome.css"
              ));
            bundles.Add(new StyleBundle("~/content/site").Include(
                "~/Content/Plugin/alertifyjs/css/alertify.css",
                "~/Content/Plugin/alertifyjs/css/themes/default.css",
               "~/Content/Css/blog-site.css",
               "~/Content/app/css/hx-loading.css"
               ));
            bundles.Add(new StyleBundle("~/content/blog").Include(
              "~/Content/app/css/blog-default.css"
              ));
            #endregion
        }
    }
}
