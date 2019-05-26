using Hx.Common.Cache;
using Hx.Common.Logs;
using Hx.Framework;
using HxBlogs.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace HxBlogs.WebApp.Controllers
{
    public class BaseController : Controller
    {
        public ILogger Logger
        {
            get
            {
                return ContainerManager.Resolve<ILogger>();
            }
        }
        protected T FillAddModel<T>(T model) where T : BaseEntity
        {
            User userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.UserId = userInfo.Id;
                model.UserName = userInfo.UserName;
            }
            return model;
        }
        protected T FillDeleteModel<T>(T model) where T : BaseEntity
        {
            User userInfo = UserContext.LoginUser;
            if (userInfo != null)
            {
                model.DeleteId = userInfo.Id;
            }
            return model;
        }
    }
}