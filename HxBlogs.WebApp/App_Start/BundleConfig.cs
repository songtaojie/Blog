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
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include(
                        "~/scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate*",
                        "~/scripts/local/messages_zh.js",
                        "~/scripts/app/hx.validator.js"));
            bundles.Add(new ScriptBundle("~/bundles/scroll").Include(
                        "~/scripts/app/scrollReveal.js"));
            #endregion

            RegisterSite(bundles);
            RegisterBlog(bundles);
            RegisterEditor(bundles);
            RegisterLogin(bundles);
            RegisterPlugin(bundles);
        }
        #region 网站所用到的js和样式文件
        /// <summary>
        /// 注册网站必须的js和css
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterSite(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/scripts/jquery-{version}.js",
                        "~/scripts/app/jquery.extendsion.js",
                       "~/scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/content/site").Include(
               "~/content/bootstrap.css",
               "~/font/font-awesome.css",
               "~/font/hx-font.css",
               "~/content/app/hx-site.css"
              ));
        }

        /// <summary>
        /// 注册网站所需的
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterBlog(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                     "~/scripts/app/hx.core.js"));

            bundles.Add(new StyleBundle("~/content/darkblack").Include(
             "~/content/app/themes/hx-default.css"
             ));
            bundles.Add(new StyleBundle("~/content/default").Include(
             "~/content/app/blog-default.css"
             ));
        }

        #endregion

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

        #region 注册所用到的插件
        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterPlugin(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/remind").Include(
                     "~/plugins/blockui/jquery.blockUI.js",
                     "~/plugins/alertifyjs/alertify.js",
                     "~/plugins/alertifyjs/hx.alertify.js"));

            bundles.Add(new StyleBundle("~/content/remind").Include(
                "~/plugins/alertifyjs/css/alertify.css",
                "~/plugins/alertifyjs/css/themes/hx.alertify.css",
                "~/content/app/hx-loading.css"
            ));
            //日期和城市选择下拉框
            bundles.Add(new ScriptBundle("~/bundles/picker").Include(
                 "~/plugins/datepicker/js/bootstrap-datepicker.js",
                 "~/plugins/datepicker/locales/bootstrap-datepicker.zh-CN.min.js",
                 "~/plugins/citypicker/js/city-picker.data.js",
                 "~/plugins/citypicker/js/city-picker.js"
                 ));
            bundles.Add(new StyleBundle("~/content/picker").Include(
                  "~/plugins/datepicker/css/bootstrap-datepicker3.css",
                  "~/plugins/citypicker/css/city-picker.css"
              ));
            //文件上传
            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                "~/plugins/dropzone/js/dropzone.js"));
            bundles.Add(new ScriptBundle("~/bundles/cropper").Include(
                "~/plugins/dropzone/js/dropzone.js",
                "~/plugins/cropper/js/cropper.js",
                "~/plugins/cropper/js/jquery-cropper.min.js"));
            bundles.Add(new StyleBundle("~/content/dropzone").Include(
                 "~/plugins/dropzone/css/dropzone.css"
             ));
            bundles.Add(new StyleBundle("~/content/cropper").Include(
                 "~/plugins/dropzone/css/dropzone.css",
                 "~/plugins/cropper/css/cropper.css"
             ));
        }
        #endregion

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
