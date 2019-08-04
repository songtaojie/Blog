using Hx.Common.Helper;
using Hx.Framework;
using Hx.WebCommon;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                result.Add("message", "请上传文件!");
                return Json(result);
            }
            //判断是否是图片，并获取高度和宽度
            bool isImage = true;
            int imgWidth = 0, imgHeight = 0;
            try
            {
                Image image = Image.FromStream(imgFile.InputStream);
                imgWidth = image.Width;
                imgHeight = image.Height;
            }
            catch
            {
                isImage = false;
            }
            
            if (!isImage)
            {
                result.Add("error", new Dictionary<string, string>() { { "message", "请上传图片文件!" } });
                result.Add("message", "请上传图片文件!");
                return Json(result);
            }
            string fileName = imgFile.FileName;
            long fileLength = imgFile.InputStream.Length;

            long max = WebHelper.GetAppSettingValue(ConstInfo.maxLength, 5242880);
            if (fileLength > max)
            {
                result.Add("error", new Dictionary<string, string>() { { "message", string.Format("上传文件大小超过限制,最大上传[{0}]!", Helper.GetFileSizeDes(fileLength)) } });
                return Json(result);
            }
            //路径处理
            string fileExt = Path.GetExtension(fileName).ToLower();
            string dirPath = rootPath + "/article/" + UserContext.LoginUser.UserName + "/" + DateTime.Now.Year + "/"+ DateTime.Now.Month.ToString("00")+"/";
            //绝对路径
            string mapPath = Server.MapPath(dirPath);
            FileHelper.TryCreateDirectory(mapPath);
            //文件名
            string guid = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
            //文件全路径
            //源文件
            string sourceFileName = guid + fileExt;
            string sourceFilePath = Path.Combine(mapPath,sourceFileName);
            imgFile.SaveAs(sourceFilePath);
            Task.Run(() => {
                //缩略图文件
                if (imgWidth >= 500 && imgHeight >= 260)
                {
                    string _268FileName = string.Format("{0}_670x268{1}", guid, fileExt);
                    string _268FilePath = Path.Combine(mapPath, _268FileName);
                    ImageManager.MakeThumbnail(sourceFilePath, _268FilePath, 670, 268, ThumbnailMode.H);
                }
                if (imgWidth >= 150 && imgHeight >= 90)
                {
                    string _120FileName = string.Format("{0}_200x120{1}", guid, fileExt);
                    string _120FilePath = Path.Combine(mapPath, _120FileName);
                    ImageManager.MakeThumbnail(sourceFilePath, _120FilePath, 200, 120, ThumbnailMode.H);
                }
               
            });
            //string letter = Request.Url.Scheme + ":" + Request.Url.Authority + "/" + UserContext.LoginUser.UserName;
            //加水印
            //ImageManager.LetterWatermark(newFilePath, 16, letter, System.Drawing.Color.WhiteSmoke, WaterLocation.RB);
            result["success"] = 1;
            result["uploaded"] = true;
            result["url"] = WebHelper.GetFullUrl(WebHelper.ToRelativePath(sourceFilePath));
            return Json(result);
        }
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadAvatar()
        {
            AjaxResult result = new AjaxResult();
            HttpFileCollectionBase imgFiles = Request.Files;
            if (imgFiles.Count <= 0)
            {
                result.Success = false;
                result.Message = "请上传文件!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            HttpPostedFileBase file = imgFiles["file"];
            long fileLength = file.InputStream.Length;
            if (fileLength > 2 * 1024 * 1024)
            {
                result.Message = "文件超出限制. 最大为: 2MB.";
                result.Success = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            UserInfo logUser = UserContext.LoginUser;
            UserInfo userInfo = MapperManager.Map<UserInfo>(logUser);
            //路径处理
            string rootPath = WebHelper.GetAppSettingValue(ConstInfo.UploadPath);
            string dirPath = rootPath + "/avatar/" + userInfo.UserName + "/" +  DateTime.Now.ToString("yyyyMM") + "/";
            //绝对路径
            string mapPath = Server.MapPath(dirPath);
            FileHelper.TryCreateDirectory(mapPath);
            //文件名
            string sourceFileName = file.FileName;
            string sourceName = Path.GetFileNameWithoutExtension(sourceFileName);
            HttpPostedFileBase _160x160File = imgFiles["file1"];
            string _160x160FileName = _160x160File.FileName;
            string _160x160Path = Path.Combine(mapPath, _160x160FileName);
            _160x160File.SaveAs(_160x160Path);
            string _32FileName = sourceName + "-32x32" + Path.GetExtension(_160x160FileName);
            userInfo.AvatarUrl = Path.Combine(dirPath, _32FileName);
            //异步保存
            Task.Run(()=> 
            {
                //32x32
                string _32Path = Path.Combine(mapPath, _32FileName);
                ImageManager.MakeThumbnail(_160x160Path, _32Path, 32, 32, ThumbnailMode.Cut);
                string filePath = Path.Combine(mapPath, sourceFileName);
                file.SaveAs(filePath);
                IUserInfoService _userService = ContainerManager.Resolve<IUserInfoService>();
                _userService.UpdateEntityFields(userInfo, "AvatarUrl");
            });
            UserContext.UpdateUser(userInfo);
            result.Resultdata = WebHelper.GetFullUrl(Path.Combine(dirPath, _160x160FileName));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}