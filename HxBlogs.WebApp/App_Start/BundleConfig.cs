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
                        "~/plugins/blockui/jquery.blockUI.js",
                        "~/plugins/alertifyjs/alertify.js",
                       "~/Scripts/app/hx.core.js"));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/mdeditor").Include(
                     "~/plugins/editormd/editormd.js",
                     "~/plugins/editormd/hx-mdeditor.js",
                     "~/Scripts/app/hx-editor.js"));
            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                    "~/plugins/ckeditor/ckeditor.js",
                    "~/plugins/ckeditor/hx-ckeditor.js",
                    "~/Scripts/app/hx-editor.js"));
            #endregion

            #region 样式文件
            bundles.Add(new StyleBundle("~/content/style").Include(
               "~/Content/bootstrap.css",
               "~/font/font-awesome.css"
              ));
            bundles.Add(new StyleBundle("~/content/site").Include(
                "~/plugins/alertifyjs/css/alertify.css",
                "~/plugins/alertifyjs/css/themes/default.css",
               "~/Content/app/css/hx-site.css",
               "~/Content/app/css/hx-core.css"
               ));
            bundles.Add(new StyleBundle("~/content/login").Include(
             "~/Content/app/css/hx-login.css"
             ));
            bundles.Add(new StyleBundle("~/content/default").Include(
              "~/Content/app/css/blog-default.css"
              ));
            bundles.Add(new StyleBundle("~/content/mdeditor").Include(
              "~/plugins/editormd/css/editormd.css"
              ));
            #endregion
        }
    }
}
