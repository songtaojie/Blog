using Hx.Framework;
using HxBlogs.IBLL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户中心
    /// </summary>
    public class UserCenterController : Controller
    {
        private IUserService _userService;
        public UserCenterController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Admin/UserCenter
        public ActionResult Profiles()
        {
            HxBlogs.WebApp.Models.UserViewModel userViewModel = MapperManager.Map<Models.UserViewModel>(UserContext.LoginUser);
            return View(userViewModel);
        }
        public ActionResult SaveProfile(HxBlogs.WebApp.Models.UserViewModel userViewModel)
        {
            AjaxResult result = new AjaxResult();
            if (ModelState.IsValid)
            {
                User logUser = UserContext.LoginUser;
                HxBlogs.Model.User user = MapperManager.Map<User>(logUser);
                user = MapperManager.Map<User>(userViewModel);
                user.Id = logUser.Id;
                user.UserName = logUser.UserName;
                user.Email = logUser.Email;
                user.PassWord = logUser.PassWord;
                List<string> fields = new List<string>();
                fields.AddRange(new string[] { "NickName", "RealName", "QQ", "WeChat", "Mobile", "Birthday",  "Address", "Description", "LastModifyTime" });
                if (string.IsNullOrEmpty(logUser.Gender))
                {
                    fields.Add("Gender");
                }
                _userService.UpdateEntityFields(user, fields);
                UserContext.UpdateUser(user);
            }
            else
            {
                result.Success = false;
                foreach (var key in ModelState.Keys)
                {
                    var modelstate = ModelState[key];
                    if (modelstate.Errors.Any())
                    {
                        result.Message = modelstate.Errors.FirstOrDefault().ErrorMessage;
                        break;
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}