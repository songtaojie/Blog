using Hx.Common.Helper;
using Hx.Framework;
using Hx.WebCommon;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Controllers
{
    public class FileController : BaseController
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Upload()
        {
            HttpPostedFileBase imgFile = Request.Files["upload"];
            string rootPath = WebHelper.GetAppSettingValue(ConstInfo.UploadPath);
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("uploaded ", false);
            //定义允许上传的文件扩展名
            //Hashtable extTable = new Hashtable();
            //extTable.Add("image", "gif,jpg,jpeg,png,bmp");

            ////最大文件大小
            //int maxSize = 1000000;
            if (imgFile == null)
            {
                result.Add("error", new Dictionary<string, string>() { { "message", "请上传文件!" } });
                return Json(result);
            }
            string fileName = imgFile.FileName;
            bool isImage = WebHelper.IsImage(imgFile.InputStream);
            if (!isImage)
            {
                result.Add("error", new Dictionary<string, string>() { { "message", "请上传图片文件!" } });
                return Json(result);
            }
            long fileLength = imgFile.InputStream.Length;
            
            long max = WebHelper.GetAppSettingValue(ConstInfo.maxLength, 5242880);
            if (fileLength > max)
            {
                result.Add("error", new Dictionary<string, string>() { { "message", string.Format("上传文件大小超过限制,最大上传[{0}]!", Helper.GetFileSizeDes(fileLength)) } });
                return Json(result);
            }
            //路径处理
            string fileExt = Path.GetExtension(fileName).ToLower();
            string dirPath = rootPath + "/" + UserContext.LoginUser.UserName + "/blog/"+ DateTime.Now.ToString("yyyyMM") + "/";
            //绝对路径
            string mapPath = Server.MapPath(dirPath);
            FileHelper.TryCreateDirectory(mapPath);
            //文件名
           
            string guid = DateTime.Now.ToString("yyyyMMddHHmmss")+ DateTime.Now.Millisecond;
            string sourceFileName = guid + "_" + fileName;
            string newFileName = string.Format("{0}{1}", guid, fileExt);
            //文件全路径
            string sourceFilePath = mapPath + sourceFileName;
            string newFilePath = mapPath + newFileName;
            imgFile.SaveAs(sourceFilePath);
            imgFile.SaveAs(newFilePath);
            // ImageManager.MakeThumbnail(sourceFilePath, newFilePath, 100, 125, ThumbnailMode.Cut);
            string letter = Request.Url.Scheme +":"+ Request.Url.Authority+"/" + UserContext.LoginUser.UserName;
            //加水印
            ImageManager.LetterWatermark(newFilePath, 16, letter, System.Drawing.Color.WhiteSmoke, WaterLocation.RB);

            result["uploaded"] = true;
            result["url"] = WebHelper.ToRelativePath(newFilePath);
            return Json(result);
        }

        public ActionResult UploadAvatar()
        {
            AjaxResult result = new AjaxResult();
            HttpFileCollectionBase imgFiles = Request.Files;
            if (imgFiles.Count <= 0) {
                result.Success = false;
                result.Message = "请上传文件!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            UserInfo logUser = UserContext.LoginUser;
            UserInfo userInfo = MapperManager.Map<UserInfo>(logUser);
            //路径处理
            string rootPath = WebHelper.GetAppSettingValue(ConstInfo.UploadPath);
            string dirPath = rootPath + "/" + userInfo.UserName + "/avatar/"+ DateTime.Now.ToString("yyyyMM") + "/";
            //绝对路径
            string mapPath = Server.MapPath(dirPath);
            FileHelper.TryCreateDirectory(mapPath);
            string avatarUrl = string.Empty;
            //文件名
            foreach (string key in imgFiles)
            {
                HttpPostedFileBase file = imgFiles[key];
                long fileLength = file.InputStream.Length;
                if (fileLength > 2 * 1024 * 1024)
                {
                    result.Message = "文件超出限制. 最大为: 2MB.";
                    result.Success = false;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                string filePath = Path.Combine(mapPath, file.FileName);
                if (file.FileName.Contains("50x50"))
                {
                    avatarUrl = Path.Combine(dirPath, file.FileName);
                }
                Task.Run(() =>
                {
                    file.SaveAs(filePath);
                });
            }
            userInfo.AvatarUrl = avatarUrl;
            result.Resultdata = WebHelper.GetFullUrl(avatarUrl);
            IUserInfoService _userService = ContainerManager.Resolve<IUserInfoService>();
            _userService.UpdateEntityFields(userInfo, "AvatarUrl");
            UserContext.UpdateUser(userInfo);
            //HttpPostedFileBase file = Request.Files["file"];
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}