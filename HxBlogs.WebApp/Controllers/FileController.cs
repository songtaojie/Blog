using Common.Helper;
using Common.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            string rootPath = Common.Config.ConfigManager.GetAppSettingValue(ConstInfo.UploadPath);
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
            
            long max = Common.Config.ConfigManager.GetAppSettingValue(ConstInfo.maxLength, 5242880);
            if (fileLength > max)
            {
                result.Add("error", new Dictionary<string, string>() { { "message", string.Format("上传文件大小超过限制,最大上传[{0}]!", Helper.GetFileSizeDes(fileLength)) } });
                return Json(result);
            }
            //路径处理
            string fileExt = Path.GetExtension(fileName).ToLower();
            string dirPath = rootPath + "/" + UserContext.LoginUser.UserName + "/image/";
            string ymd = DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            string sourceDirPath = dirPath + "sourceImage/" + ymd + "/";
            string newDirPath = dirPath + "newImage/" + ymd + "/";
            //绝对路径
            string sourceMapPath = Server.MapPath(sourceDirPath);
            string newMapPath = Server.MapPath(newDirPath);
            FileHelper.TryCreateDirectory(sourceMapPath);
            FileHelper.TryCreateDirectory(newMapPath);

            //文件名
            string sourceFileName = Guid.NewGuid() + "_" + fileName;
            string newFileName = string.Format("{0}{1}", Guid.NewGuid(), fileExt);
            //文件全路径
            string sourceFilePath = sourceMapPath + sourceFileName;
            string newFilePath = newMapPath + newFileName;
            ////imgFile.SaveAs(filePath);
            imgFile.SaveAs(sourceFilePath);
            ImageManager.MakeThumbnail(sourceFilePath, newFilePath, 100, 125, ThumbnailMode.Cut);
            string letter = Request.Url.Scheme +":"+ Request.Url.Authority+"/" + UserContext.LoginUser.UserName;
            ImageManager.LetterWatermark(newFilePath, 8, letter, System.Drawing.Color.WhiteSmoke, WaterLocation.RB);
            ////获取图片
            //Image image = System.Drawing.Image.FromStream(imgFile.InputStream);
            //var percentImage = PercentImage(image);
            //Compress(percentImage, filePath, 50);

            //String fileUrl = savePath + "image/" + ymd + "/" + newFileName;
            result["uploaded"] = true;
            result["url"] = WebHelper.ToRelativePath(newFilePath);
            return Json(result);
        }
    }
}