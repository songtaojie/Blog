using HxBlogs.IBLL;
using HxBlogs.IDAL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HxBlogs.BLL
{
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public bool Exist(string userName)
        {
            UserInfo info = this._dal.QueryUserByName(userName);
            return info != null;
        }

    }
}
