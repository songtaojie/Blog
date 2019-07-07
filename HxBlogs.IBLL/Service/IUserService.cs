using HxBlogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HxBlogs.IBLL
{
    public partial interface IUserService : IBaseService<UserInfo>
    {
        /// <summary>
        /// 根据用户名判断是否存在该用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool Exist(string userName);
        /// <summary>
        ///向数据库中插入用户并输出返回的结果信息
        /// </summary>
        /// <param name="info">要插入的用户</param>
        /// <param name="result">结果信息</param>
        /// <returns></returns>
        UserInfo Insert(UserInfo info, out AjaxResult result);
    }
}
