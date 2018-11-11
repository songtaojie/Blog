using HxBlogs.IDAL;
using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.DAL
{
    public partial class UserInfoDal : BaseDal<UserInfo>, IUserInfoDal
    {
        /// <summary>
        /// 根据用户名获取用户,忽略字母的大小写
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserInfo QueryUserByName(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            var item = from u in base.Context.Set<UserInfo>()
                       where u.UserName.ToLower() == username.ToLower()
                       select u;
            return item.FirstOrDefault();
        }
    }
}
