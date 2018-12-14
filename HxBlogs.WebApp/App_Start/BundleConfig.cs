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
                        "~/Content/Scripts/jquery-{version}.js",
                        "~/Content/App/js/jquery.extendsion.js"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/Content/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/scroll").Include(
                        "~/Content/App/js/scrollReveal.js"));
            bundles.Add(new ScriptBundle("~/bundles/metronic").Include(
                        "~/Content/Plugin/Metronic/js/app.js"));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Scripts/bootstrap.js",
                      "~/Content/Scripts/respond.js"));
            #endregion

            #region 样式文件
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/Css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Metronic/css").Include(
                      "~/Content/Plugin/Metronic/style/style-metro.css",
                      "~/Content/Plugin/Metronic/style/style.css",
                      "~/Content/Plugin/Metronic/style/style-responsive.css"));
            #endregion
        }
    }
}
