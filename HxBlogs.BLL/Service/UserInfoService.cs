using HxBlogs.IBLL;
using HxBlogs.IDAL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Common.Security;

namespace HxBlogs.BLL
{
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public bool Exist(string userName)
        {
            UserInfo info = this._dal.QueryUserByName(userName);
            return info != null;
        }
        public override UserInfo BeforeInsert(UserInfo model)
        {
            //密码加密
            model.PassWord = SafeHelper.MD5TwoEncrypt(model.PassWord);
            return base.BeforeInsert(model);
        }
    }
}
