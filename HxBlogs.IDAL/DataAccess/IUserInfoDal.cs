using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.IDAL
{
    public partial interface IUserInfoDal : IBaseDal<UserInfo>
    {
        /// <summary>
        /// 根据用户名获取用户，不区分大小写
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        UserInfo GetUserByName(string username);
    }
}
