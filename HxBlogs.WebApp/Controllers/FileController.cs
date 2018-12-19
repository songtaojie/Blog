using System;
using System.Collections.Generic;
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
            HttpPostedFileBase file = Request.Files["upload"];
            string rootPath = WebHelper.GetAppSetValue("uploadRootPath");
            //定义允许上传的文件扩展名
            //Hashtable extTable = new Hashtable();
            //extTable.Add("image", "gif,jpg,jpeg,png,bmp");

            ////最大文件大小
            //int maxSize = 1000000;

            //HttpPostedFileBase imgFile = Request.Files["imgFile"];
            //if (imgFile == null)
            //{
            //    return Content("error|请选择文件。");
            //}

            //String dirPath = Server.MapPath(savePath);
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //}

            //String dirName = Request.QueryString["dir"];
            //if (String.IsNullOrEmpty(dirName))
            //{
            //    dirName = "image";
            //}
            //if (!extTable.ContainsKey(dirName))
            //{
            //    return Content("error|目录名不正确。");
            //}

            //String fileName = imgFile.FileName;
            //String fileExt = Path.GetExtension(fileName).ToLower();

            //if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            //{
            //    return Content("error|上传文件大小超过限制。");
            //}

            //if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            //{
            //    return Content("error|上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            //}

            ////创建文件夹
            //dirPath += dirName + "/";
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //}
            //String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            //dirPath += ymd + "/";
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            //}

            //String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            //String filePath = dirPath + newFileName;

            ////imgFile.SaveAs(filePath);

            ////获取图片
            //Image image = System.Drawing.Image.FromStream(imgFile.InputStream);
            //var percentImage = PercentImage(image);
            //Compress(percentImage, filePath, 50);

            //String fileUrl = savePath + "image/" + ymd + "/" + newFileName;
            return Json(new
            {
                uploaded = true,
                url = "/upload/360wallpaper2015110162333.jpg"
            });
        }
    }
}