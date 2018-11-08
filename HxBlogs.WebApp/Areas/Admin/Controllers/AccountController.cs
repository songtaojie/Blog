using HxBlogs.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private IUserInfoService _userService;
        public AccountController(IUserInfoService userService)
        {
            this._userService = userService;
        }
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        #region 验证用户输入的信息
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckUserName(string userName)
        {
            bool result = _userService.Exist(u => u.UserName == userName);
            if (result)
            {
                return Json(string.Format("{0}已存在!", userName));
            }
            return Json(true);
        }
        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            bool result = _userService.Exist(u => u.Email == email);
            if (result)
            {
                return Json("此邮箱已被注册!");
            }
            return Json(true);
        }
        #endregion
    }
}