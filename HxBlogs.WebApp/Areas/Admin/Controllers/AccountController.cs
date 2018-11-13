using Common.Email;
using Common.Logs;
using Common.Map;
using Common.Memcached;
using HxBlogs.Framework;
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

namespace HxBlogs.WebApp.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private IUserInfoService _userService;
        public ILogger Logger
        {
            get {
                return ContainerManager.Resolve<ILogger>();
            }
        }
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
        #region 用户登录注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RegisterUser(RegisterViewModel info)
        {
            ReturnResult result = new ReturnResult();
            UserInfo userInfo = null;
            bool success = true;
            if (ModelState.IsValid)
            {
                //服务器端名字验证
                success = !(_userService.Exist(info.UserName));
                if (!success)
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("用户名{0}已存在!");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //服务器端邮箱验证
                success = !(_userService.Exist(u => u.Email == info.Email));
                if (!success)
                {
                    result.IsSuccess = false;
                    result.Message = "此邮箱已被注册!";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                await SendEmail(info.UserName, info.Email);
                userInfo = MapperHelper.Map<UserInfo>(info);
                userInfo = this._userService.Insert(userInfo);
            }
            if (success)
            {
                result.IsSuccess = true;
                result.Message = "已发送激活链接到邮箱，请尽快激活。";
                //成功以后直接到主页，即在登录状态
                string sessionId = Guid.NewGuid().ToString();
                Response.Cookies[CookieInfo.SessionID].Value = sessionId.ToString();
                string jsonData = JsonConvert.SerializeObject(userInfo);
                MemcachedHelper.Set(sessionId, jsonData, DateTime.Now.AddMinutes(20));
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "注册账户失败!";
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
                Request.Url.Port + "/Admin/Account/Activation?key=" + key;
            await Task.Run(() =>
            {
                EmailHelper email = new EmailHelper()
                {
                    MailPwd = "tao58568470jie",
                    MailFrom = "stjworkemail@163.com",
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

        public ActionResult Login()
        {
            return View();
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
        #region 激活用户
        [HttpGet]
        public ActionResult Activation()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ActiveUser(string key)
        {
            ReturnResult result = new ReturnResult();
            if (MemcachedHelper.Get(key) != null)
            {
                string userName = MemcachedHelper.Get(key).ToString();
                UserInfo userInfo = this._userService.QueryEntity(u => u.UserName == userName);
                if (userInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "此用户没有注册!";
                }
                else
                {
                    userInfo.IsActivate = "Y";
                    this._userService.Update(userInfo);
                    result.IsSuccess = true;
                }
                MemcachedHelper.Delete(key);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "此激活链接已失效!";
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}