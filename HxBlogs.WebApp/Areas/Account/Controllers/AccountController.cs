﻿using Hx.Common.Cache;
using Hx.Common.Email;
using Hx.Common.Helper;
using Hx.Common.Logs;
using Hx.Framework;
using Hx.WebCommon;
using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.WebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HxBlogs.WebApp.Areas.Account.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserService _userService;
        public ILogger Logger
        {
            get
            {
                return ContainerManager.Resolve<ILogger>();
            }
        }
        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }
       
        #region 用户登录
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoLogin()
        {
            AjaxResult result = new AjaxResult();
            result = ValidateCode(result);
            if (!result.Success) return Json(result, JsonRequestBehavior.AllowGet);
            result = ValidateUser(result);
            if (!result.Success) return Json(result, JsonRequestBehavior.AllowGet);
            string returnUrl = Request[ConstInfo.returnUrl];
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                result.Resultdata = returnUrl.Trim();
            }
            return Json(result, string.Empty, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private AjaxResult ValidateCode(AjaxResult result)
        {
            string validateCode = Request["ValidateCode"];
            string code = Session[ConstInfo.VCode].ToString();
            if (!Helper.AreEqual(code, validateCode))
            {
                result.Success = false;
                result.Message = "验证码不正确";
            }
            return result;
        }

        /// <summary>
        /// 验证用户名密码是否正确
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private AjaxResult ValidateUser(AjaxResult result)
        {
            string userName = Request["UserName"];
            string pwd = Hx.Common.Security.SafeHelper.MD5TwoEncrypt(Request["PassWord"]);
            UserInfo userInfo = this._userService.GetEntity(u => u.PassWord == pwd && (u.UserName == userName || u.Email == userName));
            if (userInfo == null)
            {
                Session[ConstInfo.VCode] = null;
                result.Success = false;
                result.Message = "用户名或密码错误!";
            }
            else
            {
                UserContext.CacheUserInfo(userInfo, Helper.IsYes(Request["Remember"]));
            }
            return result;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            UserContext.ClearUserInfo();
            return Redirect("/login");
        }

        #endregion

        #region 用户注册

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register(string returnUrl)
        {
            if (!Config.SystemConfig.AllowRegister) throw new NotFoundException("页面不存在!");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            return View();
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RegisterUser(UserInfoDTO info)
        {
            AjaxResult result = new AjaxResult();
            UserInfo userInfo = null;
            if (ModelState.IsValid)
            {
                userInfo = MapperManager.Map<UserInfo>(info);
                userInfo = this._userService.Insert(userInfo, out result);
            }
            if (result.Success)
            {
                await SendEmail(info.UserName, info.Email);
                result.Message = "已发送激活链接到邮箱，请尽快激活。";
                result.Resultdata = Request[ConstInfo.returnUrl];
                string sessionId = Guid.NewGuid().ToString();
                Response.Cookies[ConstInfo.SessionID].Value = sessionId.ToString();
                string jsonData = JsonConvert.SerializeObject(userInfo);
                MemcachedHelper.Set(sessionId, jsonData, DateTime.Now.AddMinutes(20));
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 发送邮件，激活账号
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="toEmail"></param>
        /// <returns></returns>
        public async Task<bool> SendEmail(string userName, string toEmail)
        {
            Guid key = Guid.NewGuid();
            MemcachedHelper.Set(key.ToString(), userName, DateTime.Now.AddMinutes(30));
            var checkUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" +
                Request.Url.Port + "/uc/account/activation?key=" + key;
            string mailAccount = WebHelper.GetAppSettingValue("MailAccount")
                , mailPwd = WebHelper.GetAppSettingValue("");
            await Task.Run(() =>
            {
                EmailHelper email = new EmailHelper()
                {
                    MailPwd = mailPwd,
                    MailFrom = mailAccount,
                    MailSubject = "欢迎您注册 海星·博客",
                    MailBody = EmailHelper.TempBody(userName, "请复制打开链接(或者右键新标签中打开)，激活账号",
                      "<a style='word-wrap: break-word;word-break: break-all;' href='" + checkUrl + "'>" + checkUrl + "</a>"),
                    MailToArray = new string[] { toEmail }
                };
                email.SendAsync((s, ex) => {
                    if (!s && ex != null)
                    {
                        Logger.Error("邮箱发送失败", ex);
                    }
                });
            });
            return true;
        }


        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowValidateCode()
        {
            ValidateCode validate = new ValidateCode()
            {
                MultValue = 2,
                NPhase = Math.PI
            };
            string code = validate.GetRandomNumberString(4);
            Session[ConstInfo.VCode] = code;
            byte[] bytes = validate.CreateValidateGraphic(code);
            return File(bytes, "image/jpeg");
        }

        
        #endregion

        #region 验证用户输入的信息
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckUserName(string userName)
        {
            bool result = _userService.Exist(userName);
            if (result)
            {
                return Json(string.Format("用户名{0}已存在!", userName));
            }
            return Json(true);
        }
        /// <summary>
        /// 检查邮箱是否被注册了
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 检查验证码是否正确
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckCode(string validateCode)
        {
            string code = Session[ConstInfo.VCode].ToString();
            // ReturnResult result = new ReturnResult();
            if (Helper.AreEqual(code, validateCode))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 激活用户
        [HttpGet]
        public ActionResult Activation()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ActiveUser(string key)
        {
            AjaxResult result = new AjaxResult();
            if (MemcachedHelper.Get(key) != null)
            {
                string userName = MemcachedHelper.Get(key).ToString();
                UserInfo userInfo = this._userService.GetEntity(u => u.UserName == userName);
                if (userInfo == null)
                {
                    result.Success = false;
                    result.Message = "此用户没有注册!";
                }
                else
                {
                    userInfo.Activate = "Y";
                    this._userService.Update(userInfo);
                    result.Success = true;
                }
                MemcachedHelper.Delete(key);
            }
            else
            {
                result.Success = false;
                result.Message = "此激活链接已失效!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}