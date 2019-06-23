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
                        "~/scripts/jquery-{version}.js",
                        "~/scripts/app/jquery.extendsion.js"));
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate*",
                        "~/scripts/local/messages_zh.js"));
            bundles.Add(new ScriptBundle("~/bundles/scroll").Include(
                        "~/scripts/app/scrollReveal.js"));
            #endregion

            #region 样式文件
            
            
            bundles.Add(new StyleBundle("~/content/default").Include(
              "~/content/app/blog-default.css"
              ));

            #endregion
            RegisterSite(bundles);
            RegisterBoot(bundles);
            RegisterEditor(bundles);
            RegisterLogin(bundles);
        }

        #region 登录注册
        private static void RegisterLogin(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                       "~/scripts/app/login.js"));
            bundles.Add(new ScriptBundle("~/bundles/poplogin").Include(
                       "~/scripts/app/login.js",
                       "~/scripts/app/pop.login.js"));
            bundles.Add(new StyleBundle("~/content/login").Include(
             "~/content/app/hx-login.css"
             ));
        }
        #endregion
        /// <summary>
        /// 注册bootstrap
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterBoot(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/boot").Include(
                       "~/scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/content/boot").Include(
               "~/content/bootstrap.css",
               "~/font/font-awesome.css",
               "~/font/hx-font.css"
              ));
        }
        /// <summary>
        /// 注册网站所需的
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterSite(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/plugins/alertifyjs/alertify.js",
                     "~/scripts/app/hx.core.js"));

            bundles.Add(new ScriptBundle("~/bundles/load").Include(
                      "~/plugins/blockui/jquery.blockUI.js"));

            bundles.Add(new StyleBundle("~/content/load").Include(
             "~/content/app/hx-loading.css"
             ));
            bundles.Add(new StyleBundle("~/content/site").Include(
              "~/plugins/alertifyjs/css/alertify.css",
              "~/plugins/alertifyjs/css/themes/default.css",
             "~/content/app/hx-site.css"
             ));
        }
        /// <summary>
        /// 注册编辑器所需脚本、样式
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterEditor(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/mdeditor").Include(
             "~/plugins/editormd/editormd.js",
             "~/plugins/editormd/hx-mdeditor.js",
             "~/scripts/app/hx-editor.js"));
            bundles.Add(new ScriptBundle("~/bundles/mdpreview").Include(
                    "~/plugins/editormd/lib/marked.min.js",
                    "~/plugins/editormd/lib/prettify.min.js",
                    "~/plugins/editormd/lib/raphael.min.js",
                    "~/plugins/editormd/lib/underscore.min.js",
                    "~/plugins/editormd/lib/sequence-diagram.min.js",
                    "~/plugins/editormd/lib/flowchart.min.js",
                    "~/plugins/editormd/lib/jquery.flowchart.min.js",
                    "~/plugins/editormd/editormd.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                    "~/plugins/ckeditor/ckeditor.js",
                    "~/plugins/ckeditor/hx-ckeditor.js",
                    "~/scripts/app/hx-editor.js"));

            bundles.Add(new StyleBundle("~/content/mdeditor").Include(
              "~/plugins/editormd/css/editormd.css"
              ));

            bundles.Add(new StyleBundle("~/content/mdpreview").Include(
                "~/plugins/editormd/css/editormd.preview.css"
             ));
        }

    }
}
