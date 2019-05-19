using HxBlogs.IBLL;
using HxBlogs.IDAL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Hx.Common.Security;

namespace HxBlogs.BLL
{
    public partial class UserService : BaseService<User>, IUserService
    {
        public bool Exist(string userName)
        {
            User info = this._dal.QueryUserByName(userName);
            return info != null;
        }
        public override User BeforeInsert(User model)
        {
            //密码加密
            model.PassWord = SafeHelper.MD5TwoEncrypt(model.PassWord);
            if (string.IsNullOrEmpty(model.NickName))
            {
                model.NickName = model.UserName;
            }
             
            return base.BeforeInsert(model);
        }

        public User Insert(User info, out AjaxResult returnResult)
        {
            AjaxResult result = new AjaxResult();
            bool success = false;
            if (info == null)
            {
                result.Message = "用户插入失败!";
            }
            else if (this.Exist(info.UserName))
            {
                result.Message = "用户名已存在!";
            }
            else if (this.Exist(u => u.Email == info.Email))
            {
                result.Message = "此邮箱已被注册!";
            }
            else
            {
                success = true;
            }
            returnResult = result;
            returnResult.Success = success;
            if (success)
            {
                info = this.Insert(info);
            }
            return info;
        }
    }
}
